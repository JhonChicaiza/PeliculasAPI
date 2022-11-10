import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { marker } from 'leaflet';
import { Coordenada } from 'src/app/utilidades/mapa/coordenada';
import { parsearErroresAPI } from 'src/app/utilidades/utilidades';
import { cineCreacionDTO, cineDTO } from '../cine';
import { CinesService } from '../cines.service';

@Component({
  selector: 'app-editar-cine',
  templateUrl: './editar-cine.component.html',
  styleUrls: ['./editar-cine.component.css']
})
export class EditarCineComponent implements OnInit {

  constructor(private router: Router, 
    private cinesService: CinesService,
    private activatedRoute: ActivatedRoute) { }

  //propiedad modelo
  modelo: cineDTO;
  errores: string[] = [];

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {  //lee los parametros de la URL
    this.cinesService.obtenerPorId(params.id) // y busca el parametro id
    .subscribe(cine => {
      this.modelo = cine;
    }, () => this.router.navigate(['/cines']))
    });
  }

  guardarCambios(cine: cineCreacionDTO){
    this.cinesService.editar(this.modelo.id, cine)
    .subscribe(() =>{
    this.router.navigate(['/cines']);
    }, error => this.errores = parsearErroresAPI(error));
  }

}
