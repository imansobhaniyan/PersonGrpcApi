using Microsoft.EntityFrameworkCore;

using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.Data.EF;
using PersonGrpcApi.Data.InMemory;
using PersonGrpcApi.Services;

var builder = WebApplication.CreateBuilder(args);

switch (builder.Configuration["Storage"])
{
    case "memory":
        builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        break;
    case "ef":
        builder.Services.AddScoped<IUnitOfWork, PersonDbContext>();

        builder.Services.AddDbContext<PersonDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        break;
    default:
        Console.Error.WriteLine();
        throw new Exception("Invlaid Storage Value");
}

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<PersonService>();

app.Run();
