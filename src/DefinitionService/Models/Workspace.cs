namespace DefinitionService.Models;

public record Workspace
{
  public Guid Id {get; init;}
  public string Name {get; init;} = string.Empty;
  public List<WorkspaceUser> Members {get; init;} = new();
}
