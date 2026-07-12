namespace OrchestrationEngine.Execution;

// Inherit from BackgroundService so .NET knows to run this constantly in the background!
public class StepWorker : BackgroundService
{
    private readonly StepExecutionPipeline _pipeline;
    private readonly ILogger<StepWorker> _logger;

    // Inject the Conveyor Belt into the Worker!
    public StepWorker(StepExecutionPipeline pipeline, ILogger<StepWorker> logger)
    {
        _pipeline = pipeline;
        _logger = logger;
    }

    // This method starts the second the application launches, and runs forever
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("👷 Worker is standing by the conveyor belt...");

        // Constantly pull items off the belt. If the belt is empty, this safely pauses and waits.
        await foreach (var workflowId in _pipeline.ReadAllAsync(stoppingToken))
        {
            // We got a task!
            _logger.LogInformation($"🟢 Worker picked up task for Workflow: {workflowId}");
            
            // TODO: In the future, we will call an API or execute a script here!
            await Task.Delay(1000, stoppingToken); // Simulate hard work for 1 second
            
            _logger.LogInformation($"✅ Worker finished task for Workflow: {workflowId}");
        }
    }
}
