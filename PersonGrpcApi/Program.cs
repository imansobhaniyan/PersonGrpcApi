using Microsoft.EntityFrameworkCore;

using PersonGrpcApi.Data.Abstractions;
using PersonGrpcApi.Data.EF;
using PersonGrpcApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUnitOfWork, PersonDbContext>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddDbContext<PersonDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<PersonService>();

app.Run();
