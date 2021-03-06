import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LoginService } from 'src/app/login/services/login.service';
import { Proyecto } from '../../../interfaces/proyecto.interface';
import { ProyectoService } from '../../services/proyecto.service';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-ver-proyectos',
  templateUrl: './ver-proyectos.component.html',
})

export class VerProyectosComponent implements OnInit {

  constructor(private proyectoService: ProyectoService,
              private _router: Router,
              private messageService: MessageService,
              private loginService: LoginService) 
    { 
      this.admin = this.loginService.isAdminLoggedIn();
      this.tester = this.loginService.isTesterIn();
      this.desarrollador = this.loginService.isDesarrolladorIn();
    }

  public proyectos: Proyecto[] = [];
  private idProyEliminar: number = -1;

  public admin:boolean = false;
  public tester:boolean = false; 
  public desarrollador:boolean = false;  

  ngOnInit(): void {
    this.proyectoService.getProyecto()
      .subscribe(
        ((data: Array<Proyecto>) => this.proyectos = data),
      ),
      ({ error }: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
      };
      this.cargarBreadcrumb();
  }

  home: MenuItem = { icon: 'pi pi-home', routerLink: '/' };
  public items: MenuItem[] = [];

  cargarBreadcrumb() : void {
    if(this.admin){
      this.items.push({ label: 'Proyectos', routerLink: '/proyectos' });
    }/* else if(this.tester){
    } */else{
      this.items.push({ label: 'Proyectos' });
    }
  }

  onConfirm() : void {
    this.messageService.clear();
    this.eliminar();
  }

  consultarAccion(id: number): void {
    this.idProyEliminar = id;
    this.messageService.clear();
    this.messageService.add({ key: 'c', sticky: true, severity: 'warn', summary: 'Está seguro?', detail: 'Realmente desea el Proyecto' });
  }

  onReject() {
    this.messageService.clear();
    this.idProyEliminar = -1;
  }

  eliminar(): void {
    this.proyectoService.deleteProyecto(this.idProyEliminar)
      .subscribe({
        next: data => {
          this.messageService.add({
            severity: 'success', summary: 'Listo',
            detail: 'Proyecto eliminado correctamente.'
          });
          this.proyectos = this.proyectos.filter(p => p.id !== this.idProyEliminar);
          this.idProyEliminar = -1;
        },
        error: error => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
        }
      });
  }

  editar(id: number): void {
    this._router.navigate([`/proyectos/${id}/editar`]);
  }

  asignados(id: number): void {

    this._router.navigate([`/proyectos/${id}/asignados`]);

  }

  informacion(id: number): void {

    this._router.navigate([`/proyectos/${id}/informacion`]);

  }

  incidentes(id: number): void {
    
    if(this.admin){this._router.navigate([`/proyectos/${id}/incidentes`]);}
    
    if(this.tester){this._router.navigate([`/tester/${id}/incidentes`]);}

    if(this.desarrollador){this._router.navigate([`/desarrollador/${id}/incidentes`]);}

  }

  tareas(id: number): void {
    if(this.admin) {this._router.navigate([`/proyectos/${id}/tareas`]);}
    
    if(this.tester){this._router.navigate([`/tester/${id}/tareas`]);}

    if(this.desarrollador){this._router.navigate([`/desarrollador/${id}/tareas`]);}
  }

}
