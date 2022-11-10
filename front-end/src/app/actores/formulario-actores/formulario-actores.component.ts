import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { actorCreacionDTO, actorDTO } from '../actor';

@Component({
  selector: 'app-formulario-actores',
  templateUrl: './formulario-actores.component.html',
  styleUrls: ['./formulario-actores.component.css'],
})
export class FormularioActoresComponent implements OnInit {
  constructor(private formBuilder: UntypedFormBuilder) {}

  form: FormGroup;

  @Input()
  modelo: actorDTO;

  @Input()
  errores: string[] = [];
  

  @Output()
  OnSubmit: EventEmitter<actorCreacionDTO> = new EventEmitter<actorCreacionDTO>();

  imagenCambiada = false;

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      nombre: [
        '',
        {
          validators: [Validators.required],
        },
      ],
      fechaNacimiento: '',
      foto: '',
      biografia: ''
    });

    if(this.modelo !== undefined){
      this.form.patchValue(this.modelo)
    }
  }

  cambioMarkdown(texto: string){
    this.form.get('biografia').setValue(texto);
  }

  archivoSeleccionado(file){
    this.imagenCambiada = true;
    this.form.get('foto').setValue(file);
  }

  onSubmit(){
    if (!this.imagenCambiada){
      this.form.patchValue({'foto': null}); // no envia foto si el usuario nunca edita la foto
    }
    this.OnSubmit.emit(this.form.value);
  }
}
