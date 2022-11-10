import { cineDTO } from "../cines/cine";
import { generoDTo } from "../generos/genero";

export interface PeliculaCreacionDTO{
    titulo: string;
    resumen: string;
    enCines: boolean;
    fechaLanzamiento: Date;
    trailer: string;
    poster: File;
}

//esto para poder editar las peliculas
export interface PeliculaDTO{
    titulo: string;
    resumen: string;
    enCines: boolean;
    fechaLanzamiento: Date;
    trailer: string;
    poster: string;
}

export interface PeliculaPostGet{
    genero: generoDTo[];
    cines: cineDTO[];
}