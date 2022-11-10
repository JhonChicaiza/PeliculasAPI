//using Microsoft.AspNetCore.Components;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeliculasAPI2.DTOs;
using PeliculasAPI2.Entidades;
using PeliculasAPI2.Filtros;
using PeliculasAPI2.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace PeliculasAPI2.Controllers
{
    //EndPoint
    [Route("api/generos")]
    // modelo de una accion es invalio. inmediatamente devuelve u error
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ILogger<GenerosController> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }
        //A que tipo de metodo va a responder
        [HttpGet] // api/generos
        //Aqui se crea una accion
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) // fromquery para la paginacion
        {
            var queryable = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos); // toma listado de generos y los mapea a GenerosDTO
        }

        [HttpGet ("{Id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int Id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }

            return mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id,[FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }

            genero = mapper.Map(generoCreacionDTO, genero);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) //verifico la existencia con un id
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
