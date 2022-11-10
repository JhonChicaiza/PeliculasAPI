import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { Action } from 'rxjs/internal/scheduler/Action';
import { parsearErroresAPI } from 'src/app/utilidades/utilidades';
import { actorCreacionDTO, actorDTO } from '../actor';
import { ActoresService } from '../actores.service';

@Component({
  selector: 'app-editar-actor',
  templateUrl: './editar-actor.component.html',
  styleUrls: ['./editar-actor.component.css']
})
export class EditarActorComponent implements OnInit {

  constructor(private router: Router, 
    private actoresService: ActoresService,
    private activatedRoute: ActivatedRoute) { }

  //propiedad modelo
  modelo: actorDTO;
  errores: string[] = [];

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {  //lee los parametros de la URL
    this.actoresService.obtenerPorId(params.id) // y busca el parametro id
    .subscribe(genero => {
      this.modelo = genero;
    }, () => this.router.navigate(['/generos']))
    });
  }

  guardarCambios(actor: actorCreacionDTO){
    this.actoresService.editar(this.modelo.id, actor)
    .subscribe(() =>{
    this.router.navigate(['/actores']);
    }, error => this.errores = parsearErroresAPI(error));
  }

}
