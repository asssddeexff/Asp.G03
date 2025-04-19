
using Asp.G03.Api.Extensions;
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

            builder.Services.RegisterAllServices(builder.Configuration);
            var app = builder.Build();
             await app.ConfigureMiddlewares();
            app.Run();
        }
    }
}
