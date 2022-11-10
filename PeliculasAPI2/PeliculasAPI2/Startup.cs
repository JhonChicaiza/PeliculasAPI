using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI2.Controllers;
using PeliculasAPI2.Filtros;
using PeliculasAPI2.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // servico de guardado de imagenes
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorAzureStorage>();

            // servicio de automapper
            services.AddAutoMapper(typeof(Startup));
            // para pasar Geometry factory a automapper
            services.AddSingleton(provider =>
            
                new MapperConfiguration(config =>
                {
                    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                    config.AddProfile(new AutoMapperProfiles(geometryFactory));
                }).CreateMapper());

            // Geometry factory permite trabajar con distancias el nettopologysuite
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326)); // valor para hacer mediciones en el planeta tierra

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"),
                sqlServer => sqlServer.UseNetTopologySuite()));

            services.AddCors(options =>
            {
                var frontendURL = Configuration.GetValue<string>("frontend_url");
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });

            // Permite indicar que el usuario tenga ciertas credenciales para acceder a una accion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            // Ejecuta cuando intentemos enviar una peticion http hacia un endpoint que se corresponda co dicha accion

            // aqui ponfo mi filtro global para los logs
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(filtroDeExcepcion));

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PeliculasAPI2", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeliculasAPI2 v1"));
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                // para que tengan comunicacion con el web api, diferente puerto
                app.UseCors();  

                //filtro JwtBearerDefaults
                app.UseAuthentication();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }

    }
}