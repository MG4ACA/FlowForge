using Microsoft.EntityFrameworkCore;
using DefinitionService.Data;
using DefinitionService.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options=>{
  options.AddDefaultPolicy(policy => {
    policy.WithOrigins("http://localhost:5173")
    .AllowAnyMethod()
    .AllowAnyHeader();
  });
});

builder.Services. AddDbContext<DefinitionDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/workflows", async (WorkflowDefinition request, DefinitionDbContext db)=>{
  var workflow = new WorkflowDefinition
  {
    Id = Guid.NewGuid(),
    Name = request.Name,
    Version = "1.0",
    GraphJson = request.GraphJson,
    CreatedAt = DateTimeOffset.UtcNow,
    UpdatedAt = DateTimeOffset.UtcNow
  };

  db.WorkflowDefinitions.Add(workflow);
  await db.SaveChangesAsync();

  return Results.Created($"/api/workflows/{workflow.Id}", workflow);
});

app.Run();
