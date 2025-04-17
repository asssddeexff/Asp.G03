
using Asp.G03.Api.MiddleWares;
using Domain.Contracts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Service.Abstractions;
using Services;
using Shared.ErrorModels;
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
            builder.Services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                  var errors =  actionContext.ModelState.Where(m => m.Value.Errors.Any())
                      .Select(m => new ValidationError()
                      {
                          Field = m.Key,
                          Errors=m.Value.Errors.Select(errors => errors.ErrorMessage)
                      });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);

                };

            } );

            var app = builder.Build();

            #region Seeding


            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); //Ask CLR Create Object From DbInitializer
            await dbInitializer.InitializeAsync();

            #endregion
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
