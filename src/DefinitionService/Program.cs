using Microsoft.EntityFrameworkCore;
using DefinitionService.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services. AddDbContext<DefinitionDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
