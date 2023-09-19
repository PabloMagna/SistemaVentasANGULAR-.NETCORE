import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';//usando formularios y formularios reactivos:
import { HttpClientModule } from '@angular/common/http';//usa solicitudes http:
//COMPONENTES DE ANGULAR MATERIALS:
import {MatCardModule} from '@angular/material/card'; // Para las tarjetas
import {MatInputModule} from '@angular/material/input'; // Para los cajas de txt
import {MatSelectModule} from '@angular/material/select'; // Para los desplegables
import {MatProgressBarModule} from '@angular/material/progress-bar'; // Para las barras de progreso
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner'; // Para los spinner
import {MatGridListModule} from '@angular/material/grid-list'; // Para los grid
import {LayoutModule} from '@angular/cdk/layout'; // Para los layout
import {MatToolbarModule} from '@angular/material/toolbar'; // Para las barras de herramientas
import {MatButtonModule} from '@angular/material/button'; // Para los botones
import {MatSidenavModule} from '@angular/material/sidenav'; // Para los sidenav
import {MatIconModule} from '@angular/material/icon'; // Para los iconos
import {MatListModule} from '@angular/material/list'; // Para las listas
import {MatTableModule} from '@angular/material/table'; // Para las tablas
import {MatPaginatorModule} from '@angular/material/paginator'; // Para los paginadores
import {MatDialogModule} from '@angular/material/dialog'; // Para los dialogos
import {MatSnackBarModule} from '@angular/material/snack-bar'; // Para los snack-bar
import {MatAutocompleteModule} from '@angular/material/autocomplete'; // Para los autocompletados
import {MatDatepickerModule} from '@angular/material/datepicker'; // Para los datepicker
import {MatNativeDateModule} from '@angular/material/core'; // Para los datepicker
import {MatTooltipModule} from '@angular/material/tooltip'; // Para los tooltips
import { MomentDateModule } from '@angular/material-moment-adapter';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports:[
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatCardModule,
    MatInputModule,
    MatSelectModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatSnackBarModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTooltipModule,
    MomentDateModule
  ],
  providers:[
    MatDatepickerModule,
    MomentDateModule
  ]
})
export class SharedModule { }
