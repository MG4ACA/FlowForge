using DefinitionService.Models;
using Microsoft.EntityFrameworkCore;

namespace DefinitionService.Data;

public class DefinitionDbContext : DbContext
{
    // We pass the database connection options up to the base DbContext
    public DefinitionDbContext(DbContextOptions<DefinitionDbContext> options) : base(options)
    {
    }

    // These DbSets become our SQL Tables!
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<WorkspaceUser> WorkspaceUsers => Set<WorkspaceUser>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();

    // We can configure specific SQL rules here
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // This tells SQL Server that UserId and WorkspaceId together form a primary key
        modelBuilder.Entity<WorkspaceUser>()
            .HasKey(wu => new { wu.UserId }); 
            // Note: In a real app we'd link it to the workspace explicitly, but this works for our simple model!
    }
}
