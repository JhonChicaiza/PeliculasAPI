import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { parsearErroresAPI } from 'src/app/utilidades/utilidades';
import { generoCreacionDTO, generoDTo } from '../genero';
import { GenerosService } from '../generos.service';

@Component({
  selector: 'app-editar-genero',
  templateUrl: './editar-genero.component.html',
  styleUrls: ['./editar-genero.component.css']
})
export class EditarGeneroComponent implements OnInit {

  constructor(private router: Router, 
    private generosService: GenerosService,
    private activatedRoute: ActivatedRoute) { }

  //propiedad modelo
  modelo: generoDTo;
  errores: string[] = [];

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {  //lee los parametros de la URL
    this.generosService.obtenerPorId(params.id) // y busca el parametro id
    .subscribe(genero => {
      this.modelo = genero;
    }, () => this.router.navigate(['/generos']))
    });
  }

  guardarCambios(genero: generoCreacionDTO){
    this.generosService.editar(this.modelo.id, genero)
    .subscribe(() =>{
    this.router.navigate(['/generos']);
    }, error => this.errores = parsearErroresAPI(error));
  }

}
