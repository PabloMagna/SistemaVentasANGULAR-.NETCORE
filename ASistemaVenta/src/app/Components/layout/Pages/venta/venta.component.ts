import { Component, OnInit } from '@angular/core';

import { FormBuilder,FormGroup,Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ProductoService } from 'src/app/Services/producto.service';
import { VentaService } from 'src/app/Services/venta.service';
import { UtilidadService } from 'src/app/Reutilizable/utilidad.service';
import { Producto } from 'src/app/Interfaces/producto';
import { Venta } from 'src/app/Interfaces/venta';
import { DetalleVenta } from 'src/app/Interfaces/detalle-venta';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-venta',
  templateUrl: './venta.component.html',
  styleUrls: ['./venta.component.css']
},
)
export class VentaComponent implements OnInit {
  listaProducto:Producto[]=[];
  listaPrductosFiltro:Producto[]=[]; //en realacion alo que va escrbiendo el usuario desde la anterior lista.
  listaProductosParaVenta:DetalleVenta[]=[];
  bloquearBotonRegistrar:boolean=false;
  productoSeleccionado!:Producto; //producto seleccionado de la lista.
  tipoDePagoPorDefecto:string="Efectivo";
  totalPagar:number=0;

  formularioProductoVenta:FormGroup;
  columnaTabla:string[]=['producto','cantidad','precio','total','accion'];
  datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);

  retornarProductosPorFiltro(busqueda:any):Producto[]{
    const valorBuscado = typeof busqueda === 'string' ? busqueda.toLocaleLowerCase() : busqueda.nombre.toLocaleLowerCase();
    return this.listaProducto.filter(item => item.nombre.toLocaleLowerCase().includes(valorBuscado));
  } // busqueda insensible a mayuscula, si es string busca eso o el nombre del objeto si no lo es.
  
  constructor(
    private fb:FormBuilder,
    private _productoServicio:ProductoService,
    private _ventaServicio:VentaService,
    private _utilidadServicio:UtilidadService,
  ) { 
    this.formularioProductoVenta = this.fb.group({
      producto:['',Validators.required],
      cantidad:['',Validators.required],
    });
    this._productoServicio.lista().subscribe({
      next:(data) => {
        if(data.status){
          const lista = data.value as Producto[];
          this.listaProducto = lista.filter(p => p.esActivo == 1 && p.stock > 0);
        }
      },
      error:(err) => {
        console.log(err);
      }
    });

    this.formularioProductoVenta.get('producto')?.valueChanges.subscribe(value=>{
      this.listaPrductosFiltro = this.retornarProductosPorFiltro(value);
    });
  }

  ngOnInit(): void {

  }

  mostrarProducto(producto:Producto):string{
    return producto.nombre;
  }

  productoParaVenta(event:any){
    this.productoSeleccionado = event.option.value;
    console.log('this.productoSeleccionado:', this.productoSeleccionado);
  }

  agregarProductoParaVenta(){
    console.log('productoSeleccionado en agregarProductoParaVenta:', this.productoSeleccionado);
   const _cantidad:number = this.formularioProductoVenta.value.cantidad;
   const _precio:number = parseFloat(this.productoSeleccionado.precio);
   const _total:number = _cantidad * _precio;
   this.totalPagar = this.totalPagar + _total;

   this.listaProductosParaVenta.push({
      idProducto:this.productoSeleccionado.idProducto,
      descripcionProducto:this.productoSeleccionado.nombre,
      cantidad:_cantidad,
      precioTexto:String(_precio.toFixed(2)),
      totalTexto:String(_total.toFixed(2))
    });   

    this.datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);

    this.formularioProductoVenta.patchValue({
      producto:'',
      cantidad:''
    });
  }
  
  eliminarProducto(detalle:DetalleVenta){
    this.totalPagar = this.totalPagar - parseFloat(detalle.totalTexto);
    this.listaProductosParaVenta = this.listaProductosParaVenta.filter(d => d.idProducto != detalle.idProducto);
    this.datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);
  }
  registrarVenta(){
    if(this.listaProductosParaVenta.length>0){
      this.bloquearBotonRegistrar = true;
      const request:Venta ={
        tipoPago:this.tipoDePagoPorDefecto,
        totalTexto:String(this.totalPagar.toFixed(2)),
        detalleVenta:this.listaProductosParaVenta
      }
      this._ventaServicio.registrar(request).subscribe({
        next:(response) => {
          if(response.status){
            this.totalPagar = 0.00;
            this.listaProductosParaVenta = [];
            this.datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);
            console.log('response.value:', response.value);

            Swal.fire({
              icon:'success',
              title:'Venta registrada',
              text: `Numero de venta: ${response.value.numeroDocumento}`,
            })
          }else{
            this._utilidadServicio.mostrarAlerta("No se pudo registrar la venta", "ops!");
          }
        }, complete:() => {
          this.bloquearBotonRegistrar = false;
        }, error:(err) => {
          console.log(err);
        }
      });
    }
  }
}
