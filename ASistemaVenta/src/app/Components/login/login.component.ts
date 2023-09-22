import { Component } from '@angular/core';
import { FormBuilder,FormGroup,Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from 'src/app/Interfaces/login';
import { UsuarioService } from 'src/app/Services/usuario.service';
import { UtilidadService } from 'src/app/Reutilizable/utilidad.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  formularioLogin:FormGroup;
  ocultarPassword:boolean=true;
  mostrarLoading:boolean=false;
  
  constructor(
    private fb:FormBuilder,
    private router:Router,
    private usuarioServicio:UsuarioService,
    private utilidadServicio:UtilidadService,
    ){
      this.formularioLogin=this.fb.group({
        email:['',Validators.required],
        password:['',Validators.required],
    });
  }
    iniciarSesion(){
      this.mostrarLoading=true;
      const request:Login = {
        correo: this.formularioLogin.value.email,
        clave: this.formularioLogin.value.password,
    }
    this.usuarioServicio.iniciarSesion(request).subscribe({
      next:(data)=>{
        console.log(data);
        if(data.status){
          this.utilidadServicio.guardarSesionUsuario(data.value);
          this.router.navigate(['pages'])
        }else{
          this.utilidadServicio.mostrarAlerta("No se encontraron coincidencias","Oops");
        }
      },
        complete:()=>{
          this.mostrarLoading=false;
        },
        error:()=>{
          this.utilidadServicio.mostrarAlerta("Hubo un error!","Oops");
        }
    })
  }
}
