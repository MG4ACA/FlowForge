using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using OrchestrationEngine.StateMachines;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrchestrationEngine.Data;

// Notice it inherits from SagaDbContext!
public class OrchestrationDbContext : SagaDbContext
{
    public OrchestrationDbContext(DbContextOptions<OrchestrationDbContext> options) : base(options)
    {
    }

    // We must define how the WorkflowInstance maps to the SQL Table
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new WorkflowInstanceMap(); }
    }
}

// This tells EF Core exactly how to build the table for our Pizza Tracker
public class WorkflowInstanceMap : SagaClassMap<WorkflowInstance>
{
    protected override void Configure(EntityTypeBuilder<WorkflowInstance> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.StartedAt);
    }
}
