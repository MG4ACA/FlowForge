# FlowForge: Project Progress & Challenge Tracker

**Overall Completion:** 0% (0/5 Phases Completed)

---

## Phase 1: Foundation (100% Complete)
- [x] **Challenge 1.0: Version Control Setup**
  - Initialize a Git repository.
  - Create a proper `.gitignore` file for .NET and Node.js.
- [x] **Challenge 1.1: Scaffolding the Solution**
  - Create the `.sln` file and the 3 .NET 8 Minimal API projects (`Gateway`, `DefinitionService`, `OrchestrationEngine`).
  - Add projects to the solution.
- [x] **Challenge 1.2: Docker Infrastructure**
  - Create `docker-compose.yml` for SQL Server and RabbitMQ.
- [x] **Challenge 1.3: Entity Framework Models**
  - Setup EF Core, create `WorkflowDefinition`, `Workspace`, models.
  - Run initial migrations.
- [x] **Challenge 1.4: Frontend Foundation**
  - Scaffold React with Vite (`react-ts`).
  - Install React Flow and dependencies.

## Phase 2: Orchestration Core (100% Complete)
- [x] **Challenge 2.1: MassTransit Setup**
  - Configure `WorkflowStateMachine` and Saga persistence.
- [x] **Challenge 2.2: Execution Pipeline**
  - Implement `StepExecutionPipeline` using `System.Threading.Channels`.
- [x] **Challenge 2.3: API Endpoints**
  - Build CRUD for Definition Service and Execution endpoints for Orchestration.

## Phase 3: The Visual Canvas (100% Complete)
- [x] **Challenge 3.1: React Flow Foundation**
  - Render a basic drag-and-drop canvas using React Flow.
- [x] **Challenge 3.2: Custom Nodes**
  - Build custom UI components for specific workflow steps (e.g., Email Node, Wait Node).
- [x] **Challenge 3.3: Backend Integration**
  - Save the workflow graph to the Definition Service and trigger executions via the Orchestration API.

## Phase 4: API Gateway (0% Complete)
- [ ] **Challenge 4.1: YARP Foundation**
  - Install YARP (Yet Another Reverse Proxy) in the Gateway project.
- [ ] **Challenge 4.2: Dynamic Routing**
  - Configure YARP to route Frontend traffic to the isolated Definition and Orchestration microservices.
- [ ] **Challenge 4.3: Frontend Refactoring**
  - Update the React App to only communicate with the API Gateway.

*(Phase 5 will be detailed as we progress)*
