import { Component, OnInit,AfterViewInit,ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Producto } from 'src/app/Interfaces/producto';
import { ProductoService } from 'src/app/Services/producto.service';
import{ModalProductoComponent} from '../../Modales/modal-producto/modal-producto.component';
import { UtilidadService } from 'src/app/Reutilizable/utilidad.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-producto',
  templateUrl: './producto.component.html',
  styleUrls: ['./producto.component.css']
})
export class ProductoComponent implements OnInit, AfterViewInit{
  
  columnasTabla:string[] =['nombre','categoria','stock','precio', 'estado','acciones'];
  dataInicio:Producto[]=[];
  dataListaProducto = new MatTableDataSource(this.dataInicio);
  @ViewChild(MatPaginator) paginacionTabla!:MatPaginator;

  constructor(
    private dialog:MatDialog,
    private _usuarioProducto:ProductoService,
    private _utilidadServicio:UtilidadService
  )
  { }

  obtenerProductos(){
    this._usuarioProducto.lista().subscribe({
      next:(data)=>{
        if(data.status){
          this.dataListaProducto.data = data.value;
        }else{
          this._utilidadServicio.mostrarAlerta("No se encontraron datos", "oops");
        }
      },
      error:(err)=>{
        console.log(err);
      }
    });
  }

  ngOnInit(): void {
    this.obtenerProductos();
  }

  ngAfterViewInit(): void {
    this.dataListaProducto.paginator = this.paginacionTabla;
  }

  aplicarFiltroTabla(event:Event){
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataListaProducto.filter = filterValue.trim().toLowerCase();
  }

  nuevoProducto(){
    this.dialog.open(ModalProductoComponent,{
      disableClose:true, // no se puede cerrar clicando en fuera del mismo.
    }).afterClosed().subscribe(resultado =>{
      if(resultado === "true"){
        this.obtenerProductos();
      }
    });
  }

  editarProducto(producto:Producto){
    this.dialog.open(ModalProductoComponent,{
      disableClose:true,
      data:producto
    }).afterClosed().subscribe(resultado =>{
      if(resultado === "true"){
        this.obtenerProductos();
      }
    });
  }

  eliminarProducto(producto:Producto){
    Swal.fire({
      title: '¿Estas seguro de eliminar este producto?',
      text: producto.nombre,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this._usuarioProducto.eliminar(producto.idProducto).subscribe({
          next:(data)=>{
            if(data.status){
              this._utilidadServicio.mostrarAlerta("El Producto fue eliminado", "Listo");
              this.obtenerProductos();
            }else{
              this._utilidadServicio.mostrarAlerta("No se pudo eliminar el producto", "Error");
            }
          },
          error:(err)=>{
            console.log(err);
          }
        });
      }
    })
  }
}
