
using Domain.Contracts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Service.Abstractions;
using Services;

using  AssemblyMappaing =  Services.AssemblyReference;
namespace Asp.G03.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(Options =>
            {

                // Options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            }


     );
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();//Allow DI For DbInitializer
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(AssemblyMappaing).Assembly);
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
           

            var app = builder.Build();

            #region Seeding


            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); //Ask CLR Create Object From DbInitializer
            await dbInitializer.InitializeAsync();

            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
