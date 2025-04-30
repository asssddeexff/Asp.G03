using Microsoft.AspNetCore.Mvc;
using Services;
using Persistence;
using Shared.ErrorModels;
using Asp.G03.Api.MiddleWares;
using Domain.Contracts;
using Domain.Models.identity;
using Microsoft.AspNetCore.Identity;
using Persistence.identity;
using Microsoft.Extensions.Options;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace Asp.G03.Api.Extensions
{
    public static class Extensions
    {

        public static IServiceCollection RegisterAllServices(this IServiceCollection services , IConfiguration configuration )
        {

            services.AddBuiltInServices();
            services.AddSwaggerServices();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
       


         services.AddInfrastructureServices(configuration);
            services.AddIdentityServices();
            services.AddApplicationServices(configuration);
            services.ConfigureServices();
            services.ConfigureJwtServices(configuration);




            return services;
        }


        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {


            services.AddControllers();


            return services;
        }


        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services , IConfiguration configuration)
        {


            var JwtOptions = configuration.GetSection("jwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = JwtOptions.Issuer,
                    ValidAudience = JwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey))

                };

            });



            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {


            services.AddIdentity<AppUser,IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();


            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;
        }


        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {


            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                        .Select(m => new ValidationError()
                        {
                            Field = m.Key,
                            Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                        });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);

                };

            });


            return services;
        }

        public static async Task< WebApplication> ConfigureMiddlewares(this WebApplication app)
        {

           


           await app.InitializeDataBaseAsync();

            app.UseGlobalErrorHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }


        private static async Task<WebApplication> InitializeDataBaseAsync(this WebApplication app)
        {

            


            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); //Ask CLR Create Object From DbInitializer
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();


            return app;
        }


        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {




            app.UseMiddleware<GlobalErrorHandlingMiddleware>();


            return app;
        }
    }
}
