import { Injectable } from '@angular/core';

import { MatSnackBar } from '@angular/material/snack-bar'; //alertas
import { Sesion } from '../Interfaces/sesion';

@Injectable({
  providedIn: 'root'
})
export class UtilidadService {

  constructor(private _snackBar:MatSnackBar) { }

  mostrarAlerta(mensaje:string,tipo:string){
    this._snackBar.open(mensaje,tipo,{
      duration:3000,
      horizontalPosition:'end',
      verticalPosition:'top'
    })
  }
  guardarSesionUsuario(sesion:Sesion){
    localStorage.setItem('usuario',JSON.stringify(sesion))
  }
  obtenerSesionUsuario():Sesion{
    const dataCadena = localStorage.getItem('usuario');
    return JSON.parse(dataCadena!);
  }
  eliminarSesionUsuario(){
    localStorage.removeItem('usuario');
  }
}
