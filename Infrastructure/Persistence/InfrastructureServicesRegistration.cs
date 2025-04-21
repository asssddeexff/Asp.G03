using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.identity;
using Persistence.Repositiores;
using StackExchange.Redis;

namespace Persistence
{
   public static class InfrastructureServicesRegistration
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration) 
        {

            services.AddDbContext<StoreDbContext>(Options =>
            {

                // Options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                Options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            }


   );
            services.AddDbContext<StoreIdentityDbContext>(Options =>
            {

                
                Options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));

            }


  );

            services.AddScoped<IDbInitializer, DbInitializer>();//Allow DI For DbInitializer
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            }
            );


            return services;
        }
    }
}
