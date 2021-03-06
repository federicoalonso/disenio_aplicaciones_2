using Incidentes.Datos;
using Incidentes.DatosInterfaz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Incidentes.DatosFabrica
{
    public class FabricaServiciosDatos
    {
        private readonly IServiceCollection services;
        public FabricaServiciosDatos(IServiceCollection services)
        {
            this.services = services;
        }
        public void AgregarServicios()
        {
            services.AddScoped<IRepositorioGestores, RepositorioGestores>();
           
        }
        public void AgregarContextoDatos()
        {
            string directory = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(directory)
            .AddJsonFile("appsettings.json")
            .Build();
            var connectionString = configuration.GetConnectionString(@"sqlConnection");
            
            services.AddDbContext<Contexto>(opts =>
            opts.UseSqlServer(connectionString, 
            b => b.MigrationsAssembly("Incidentes.Datos"))); 
        }
    }
}
