import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css']
})
export class RatingComponent implements OnInit {

@Input()
maximoRating = 5;
@Input()
ratingSeleccionado = 0;
@Output()
//indica que quiero disparar un evento
rated: EventEmitter<number> = new EventEmitter<number>();
//arreglo para iterar
maximoRatingArr = [];
//para saber si el usuario a votado
votado = false;
//para que quede la votacion inicial
ratingAnterior;

  constructor() { }

  ngOnInit(): void {
    this.maximoRatingArr = Array(this.maximoRating).fill(0);
  }

  manejarMouseEnter(index: number): void{
    this.ratingSeleccionado = index + 1;
  }

  manejarMouseLeave(){
    if(this.ratingAnterior !== 0){
      this.ratingSeleccionado = this.ratingAnterior;
    }else{
      this.ratingSeleccionado = 0;
    }
  }

  rate(index: number): void{
    this.ratingSeleccionado = index + 1;
    this.votado = true;
    this.ratingAnterior = this.ratingSeleccionado;
    //con esto disparo un evento rated y emito el valor a traves del evento
    this.rated.emit(this.ratingSeleccionado);
  }

}
