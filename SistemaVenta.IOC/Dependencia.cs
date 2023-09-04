using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios;
using SistemaVenta.DAL.Repositorios.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Utility;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        public static void IyectarDependencias(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<DbventaContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

        }
        
    
    }
}
