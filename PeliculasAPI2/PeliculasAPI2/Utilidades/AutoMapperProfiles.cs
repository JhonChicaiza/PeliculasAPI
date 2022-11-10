using AutoMapper;
using NetTopologySuite.Geometries;
using PeliculasAPI2.DTOs;
using PeliculasAPI2.Entidades;
using System;
using System.Collections.Generic;

namespace PeliculasAPI2.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            // mapeo de Genero a GeneroDTO y viceversa
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            //Actor
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());

            // crear cine
            CreateMap<CineCreacionDTO, Cines>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));

            // obtener cine
            CreateMap<Cines, CineDTO>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));

            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapearPeliculasActores));
                

        }

        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO,
            Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaCreacionDTO.Actores == null) { return resultado; }

            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.Id, Personaje = actor.Personaje});
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO,
            Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if(peliculaCreacionDTO.GenerosIds == null) { return resultado;}

            foreach(var id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros() {GeneroId = id});
            }

            return resultado;
        }

        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO,
            Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaCreacionDTO.CinesIds == null) { return resultado; }

            foreach (var id in peliculaCreacionDTO.CinesIds)
            {
                resultado.Add(new PeliculasCines() { CineId = id });
            }

            return resultado;
        }
    }
}
