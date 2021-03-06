import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncidentesProyectosComponent } from './pages/incidentes-proyectos/incidentes-proyectos.component';
import { IncidentesDesarrolladorComponent } from './pages/incidentes-desarrollador/incidentes-desarrollador.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PrimeNgModule } from '../prime-ng/prime-ng.module';
import { DesarrolladorComponent } from './pages/desarrollador/desarrollador.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    IncidentesProyectosComponent,
    IncidentesDesarrolladorComponent,
    DesarrolladorComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PrimeNgModule,
    SharedModule
  ],
  exports:[
    IncidentesProyectosComponent,
    IncidentesDesarrolladorComponent
  ]
})
export class ReportesModule { }
