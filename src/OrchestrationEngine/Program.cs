using MassTransit;
using OrchestrationEngine.StateMachines;
using OrchestrationEngine.Data;
using Microsoft.EntityFrameworkCore;
using OrchestrationEngine.Execution;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrchestrationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<StepExecutionPipeline>();
builder.Services.AddHostedService<StepWorker>();

builder.Services.AddMassTransit(config=>
{
  config.AddSagaStateMachine<WorkflowStateMachine, WorkflowInstance>()
  .EntityFrameworkRepository(r=>{
    r.ExistingDbContext<OrchestrationDbContext>();
    r.UseSqlServer();
  });

  config.UsingRabbitMq((context, rabbitConfig)=>
  {
    rabbitConfig.Host("localhost", "/", hostConfig=>
    {
      hostConfig.Username("guest");
      hostConfig.Password("guest");
    });
    rabbitConfig.ConfigureEndpoints(context);
  });
}
);

var app = builder.Build();

app.MapPost("/api/workflows/{workflowId}/start", async (Guid workflowId, IPublishEndpoint publishEndpoint)=>
{
  var command = new StartWorkFlowCommand(workflowId);
  await publishEndpoint.Publish(command);
  return Results.Accepted(value: $"Workflow {workflowId} has been queued for execution");
});

app.Run();
