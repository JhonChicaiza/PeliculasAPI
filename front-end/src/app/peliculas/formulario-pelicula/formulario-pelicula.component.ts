import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MultipleSelectorModel } from 'src/app/utilidades/selector-multiple/MultipleSelectorModel';
import { PeliculaCreacionDTO, PeliculaDTO } from '../pelicula';

@Component({
  selector: 'app-formulario-pelicula',
  templateUrl: './formulario-pelicula.component.html',
  styleUrls: ['./formulario-pelicula.component.css']
})
export class FormularioPeliculaComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) { }

  form: FormGroup

  @Input()
  modelo: PeliculaDTO;

  @Output()
  OnSubmit: EventEmitter<PeliculaCreacionDTO> = new EventEmitter<PeliculaCreacionDTO>();

  @Input()
  generosNoSeleccionados: MultipleSelectorModel[];
  //Para saber que generos estan a cada lado
  generosSeleccionados: MultipleSelectorModel[] = [];

  @Input()
  //Para mandar las peliculas a los generos que pertenezcan
  cinesNoSeleccionados: MultipleSelectorModel[];

  cinesSeleccionados: MultipleSelectorModel[] = [];


  ngOnInit(): void {
      this.form = this.formBuilder.group({
        titulo: [
          '',
          {
            validators: [Validators.required]
          }
        ],
        resumen: '',
        enCines: false,
        trailer: '',
        fechaLanzamiento: '',
        poster: '',
        generosId: '',
        cinesId: ''
      });

      if(this.modelo !== undefined){
        this.form.patchValue(this.modelo);
      }
  }

  changeMarkdown(texto){
    this.form.get('resumen').setValue(texto);
  }

  archivoSeleccionado(archivo: File){
    this.form.get('poster').setValue(archivo);
  }

  guardarCambios(){
    console.log(this.generosSeleccionados)
    //arreglo para pasar los ids
    const generosIds = this.generosSeleccionados.map(val => val.llave);
    this.form.get('generosId').setValue(generosIds);
    //
    const cinesIds = this.cinesSeleccionados.map(val => val.llave);
    this.form.get('cinesId').setValue(cinesIds);
    
    this.OnSubmit.emit(this.form.value);
  }

}
