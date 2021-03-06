import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Usuario } from 'src/app/interfaces/dtoUsuario.interface';
import { LoginService } from '../../services/login.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-formulario',
  templateUrl: './formulario.component.html',
  styles: [
  ]
})
export class FormularioComponent implements OnInit {

  constructor(private loginService: LoginService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private _router: Router) {  }

  ngOnInit(): void {  }

  miFormulario: FormGroup = this.fb.group({
    nombre: [, [Validators.required, Validators.minLength(3)]],
    contrasenia: [, [Validators.required, Validators.minLength(3)]],
  })

  campoEsValido(campo: string) {
    return this.miFormulario.controls[campo].errors
      && this.miFormulario.controls[campo].touched
  }

  login(): void {
    if (this.miFormulario.invalid) {
      this.miFormulario.markAllAsTouched();
      return;
    }

    const usuario: Usuario = {
      NombreUsuario: this.miFormulario.value.nombre,
      Contrasenia: this.miFormulario.value.contrasenia
    }

    this.loginService.login(usuario)
      .subscribe(
        (data: any) => {
          sessionStorage.setItem('token', data.token);
          sessionStorage.setItem('authData', JSON.stringify(data));
          this.messageService.add({ severity: 'success', summary: 'Listo', detail: 'Usuario logueado correctamente' });
          this._router.navigate(['/']);
          this.loginService.actualizarUsuario(data);
          this.miFormulario.reset();
        },
        ({ error }: any) => {
          if(error.type == "error"){
            this.messageService.add({ severity: 'error', summary: 'Error', detail: "Error de conexión con el backend." });
          }else{
            this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
          }
        }
      );
  }
}
