import { Component, OnInit } from '@angular/core';

import {Chart,registerables} from 'chart.js'; // chart.org es una libreria de barras
import { DashBoardService } from 'src/app/Services/dash-board.service';
Chart.register(...registerables); //habilitando funcionalidades específicas adicionales de Chart.js al registrar los módulos que importaste previamente.

@Component({
  selector: 'app-dash-board',
  templateUrl: './dash-board.component.html',
  styleUrls: ['./dash-board.component.css']
})
export class DashBoardComponent implements OnInit {
  totalIngresos:string="0";
  totalVentas:string="0";
  totalProductos:string="0";

  constructor(
    private _dashboardServicio:DashBoardService
  ){}

  mostrarGrafico(labelGrafico:any[], dataGrafico:any[]){ //label como las fechas y data ventas
    const chartBarras = new Chart('chartBarras',{
      type:'bar',
      data:{
        labels:labelGrafico,
        datasets:[{
          label:"# de Ventas",
          data: dataGrafico,
          backgroundColor:[
            'rgba(54, 162, 235, 0.2)',
          ],
          borderColor:[
            'rgba(54, 162, 235, 1)',
          ],
          borderWidth:1
        }]
      },
      options:{
        maintainAspectRatio:false,
        responsive:true,
        scales:{
          y:{
            beginAtZero:true
          }
        }
      }
  });
}

ngOnInit(): void {
  this._dashboardServicio.resumen().subscribe({
    next:(data) =>{
      if(data.status){
        this.totalIngresos = data.value.totalIngresos;
        this.totalVentas = data.value.totalVentas;
        this.totalProductos = data.value.totalProductos;
        
        const arrayData:any[] = data.value.ventasUltimaSemana;
        console.log(arrayData);

        const labelTemp = arrayData.map((value) => value.fecha); //datos para label del grafico, desde db
        const dataTemp = arrayData.map((value) => value.total); //datos de cant de ventas por dia, desde db
        this.mostrarGrafico(labelTemp,dataTemp);
      }else{
        console.log(data.msg);
      }
    }, error:(err) =>{
      console.log(err);
    }
  })

}


}
