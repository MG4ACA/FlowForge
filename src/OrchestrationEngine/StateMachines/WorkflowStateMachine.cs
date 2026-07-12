using MassTransit;

namespace OrchestrationEngine.StateMachines;

public record StartWorkFlowCommand(Guid CorrelationId);

public class WorkflowStateMachine: MassTransitStateMachine<WorkflowInstance>
{
  public State Running {get; private set;} =null!;
  public State Completed { get; private set;} = null!;

  public Event<StartWorkFlowCommand> StartWorkFlow {get; private set;}=null!;

  public WorkflowStateMachine(){
  InstanceState(x=> x.CurrentState);

  Initially(
    When(StartWorkFlow).Then(context=>{
      context.Saga.StartedAt = DateTimeOffset.UtcNow;
    }).TransitionTo(Running)
  );
  }
}
