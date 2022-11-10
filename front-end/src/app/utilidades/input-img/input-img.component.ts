import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { toBase64 } from '../utilidades';

@Component({
  selector: 'app-input-img',
  templateUrl: './input-img.component.html',
  styleUrls: ['./input-img.component.css']
})
export class InputImgComponent implements OnInit {

  constructor() { }

  imagenBase64: string;

  @Input()
  urlImagenActual: string;

  @Output()
  archivoSeleccionado: EventEmitter<File> = new EventEmitter<File>();

  ngOnInit(): void {
  }

  change(event){
    if(event.target.files.length > 0){
      //con esto obtengo el archivo seleccionado
      const file: File = event.target.files[0];
      //convertir archivo file a base64
      toBase64(file).then((value: string) => this.imagenBase64 = value)
      .catch(error => console.log(error));
      //envia la img al padre
      this.archivoSeleccionado.emit(file);
      //para que no asome dos img sino solo la cambiada
      this.urlImagenActual = null;
    }
  }
}
