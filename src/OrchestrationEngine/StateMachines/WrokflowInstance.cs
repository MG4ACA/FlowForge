using MassTransit;

namespace OrchestrationEngine.StateMachines;

public class WorkflowInstance: SagaStateMachineInstance
{

  public Guid CorrelationId { get; set; }
  public string CurrentState { get; set; } = string.Empty;
  public DateTimeOffset StartedAt {get; set;}
  
}
