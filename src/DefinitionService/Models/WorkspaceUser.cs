namespace DefinitionService.Models;

public record WorkspaceUser
{
  public Guid UserId {get; init;}
  public WorkspaceRole Role {get; init;}  
}
