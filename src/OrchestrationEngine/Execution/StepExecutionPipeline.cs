using System.Threading.Channels;

namespace OrchestrationEngine.Execution;

public class StepExecutionPipeline
{
  public readonly Channel<Guid> _channel;
  
  public StepExecutionPipeline(){
    var options = new UnboundedChannelOptions{
      SingleWriter = false,
      SingleReader = true
    };

    _channel = Channel.CreateUnbounded<Guid>(options);
  }

  public async ValueTask WriteAsync(Guid workflowId){
    await _channel.Writer.WriteAsync(workflowId);
  }

  public IAsyncEnumerable<Guid> ReadAllAsync(CancellationToken cancellationToken){
    return _channel.Reader.ReadAllAsync(cancellationToken);
  }
  
}
