namespace DefinitionService.Models;

public record WorkflowDefinition
{
  public Guid Id {get; init;}  
  public Guid WorkspaceId {get; init;}
  public string Name {get; init;} = string.Empty;
  public string Version {get; init;} = string.Empty;
  public string GraphJson {get; init;} = string.Empty;
  public DateTimeOffset CreatedAt {get; init;} 
  public DateTimeOffset UpdatedAt {get; init;} 
}
