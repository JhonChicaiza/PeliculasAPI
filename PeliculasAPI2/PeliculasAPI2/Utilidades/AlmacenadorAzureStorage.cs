using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PeliculasAPI2.Utilidades
{
    public class AlmacenadorAzureStorage : IAlmacenadorArchivos
    {
        private string connectionString;
        public AlmacenadorAzureStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<String> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(archivo.FileName);
            var archivoNombre = $"{Guid.NewGuid()}{extension}"; // nombre unico. evita colision de nombres
            var blob = cliente.GetBlobClient(archivoNombre);
            await blob.UploadAsync(archivo.OpenReadStream());
            return blob.Uri.ToString(); // retorna url del archivo subido
        }

        public async Task BorrarArchivo(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<String> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }
    }
}
