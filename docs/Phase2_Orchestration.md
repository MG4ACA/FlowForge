# FlowForge: Phase 2 Orchestration Documentation

This document serves as a study guide for the Orchestration Engine, which handles the execution and state management of long-running workflows.

## 1. MassTransit Sagas (State Machines)

When a workflow runs, it might take hours or days to complete. If the server crashes, we cannot lose the workflow's state. We use **MassTransit Sagas** to track the state persistently.

### The Two Components of a Saga:
1. **The Data (`WorkflowInstance`)**: Inherits from `SagaStateMachineInstance`. This represents the actual row in the database. It stores the `CorrelationId` (the unique ID of the workflow run) and the `CurrentState` (e.g., "Running", "Completed").
2. **The Logic (`WorkflowStateMachine`)**: Inherits from `MassTransitStateMachine`. This is the "Manager's Rulebook" defined in the constructor. It dictates how the workflow moves between states when certain Events/Messages are received (e.g., "When StartWorkFlow happens, transition to Running").

### EF Core Persistence
To prevent "Amnesia", we configured MassTransit to use Entity Framework Core. By creating the `OrchestrationDbContext`, MassTransit automatically saves the `WorkflowInstance` to SQL Server every time the state changes.

---

## 2. System.Threading.Channels (The Execution Pipeline)

While the State Machine manages *what* state the workflow is in, it should not do the heavy lifting of executing tasks (like sending emails or calling APIs). If it did, the State Machine would get blocked, slowing down the entire system.

### The Conveyor Belt Analogy
To solve this, we use `System.Threading.Channels`.
- A Channel is a thread-safe **Conveyor Belt**.
- The State Machine (The Manager) drops a task onto the belt and immediately goes back to managing.
- A `BackgroundService` (The Worker) stands at the end of the belt. The second a task comes down the line, the Worker grabs it and executes the heavy lifting.

### The Pipeline Structure
To implement this, we create a wrapper class around the Channel. This wrapper provides a way for the Manager to put things *on* the belt, and for the Worker to take things *off*.

Here is the exact boilerplate structure for a Channel Pipeline. You can use this pattern in any enterprise .NET application!

```csharp
using System.Threading.Channels;

namespace OrchestrationEngine.Execution;

public class StepExecutionPipeline
{
    // The physical conveyor belt that holds Guids (Task IDs)
    private readonly Channel<Guid> _channel;

    public StepExecutionPipeline()
    {
        // An Unbounded channel means the belt can hold infinite items without overflowing
        var options = new UnboundedChannelOptions
        {
            SingleWriter = false, // Multiple managers can put tasks on the belt
            SingleReader = true   // Only one worker will pull tasks off (for now)
        };
        _channel = Channel.CreateUnbounded<Guid>(options);
    }

    // 1. The Manager uses this to put a task ON the belt
    public async ValueTask WriteAsync(Guid workflowId)
    {
        await _channel.Writer.WriteAsync(workflowId);
    }

    // 2. The Worker uses this to read tasks OFF the belt (it waits automatically if the belt is empty!)
    public IAsyncEnumerable<Guid> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
```

---

## 3. The API Endpoint (The Drive-Thru Window)

Even with a Manager, a Conveyor Belt, and Workers, our Orchestration Engine is completely isolated. We need a way for the outside world (like the React Frontend) to trigger workflows.

### Minimal APIs
In .NET 8, we use **Minimal APIs** in `Program.cs` to create HTTP endpoints rapidly.
This acts as a "Drive-Thru Window" for our factory. The user sends an HTTP `POST` request to the window, and the window uses a megaphone (MassTransit's `IPublishEndpoint`) to yell at the State Machine Manager to start the workflow!

```csharp
app.MapPost("/api/workflows/{workflowId}/start", async (Guid workflowId, IPublishEndpoint publishEndpoint) =>
{
    // We create the message (The Event we defined in the State Machine!)
    var command = new StartWorkFlowCommand(workflowId);

    // We yell into the megaphone! RabbitMQ will hear this, and hand it to the State Machine Manager.
    await publishEndpoint.Publish(command);

    return Results.Accepted(value: $"Workflow {workflowId} has been queued for execution!");
});
```
