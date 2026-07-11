# FlowForge: Phase 1 Foundation Documentation

This document serves as a study guide and architectural summary of everything built in Phase 1 (Challenges 1.0 to 1.3) of the FlowForge project. 

## 1. The Microservices Architecture

Instead of building one massive application (a Monolith), we built a distributed system using three **Minimal APIs** in .NET 8. We use a `.sln` (Solution) file to group them together.

### Why Minimal APIs?
Minimal APIs remove the bloat of traditional ASP.NET MVC controllers. Everything is configured directly in `Program.cs`. This makes our microservices lightweight, fast, and easy to read.

### The Three Services:
1. **Definition Service:** (The Librarian). This service's only job is to save, update, and retrieve the structural data of the application (like Workspaces and the JSON blueprints of workflows).
2. **Orchestration Engine:** (The Worker). This service will read the JSON blueprints and actually execute the workflows step-by-step in the background.
3. **Gateway:** (The Bouncer). This service acts as a single entry point. The frontend only talks to the Gateway, which checks authentication and routes traffic to the internal services.

## 2. Docker Infrastructure

We created a `docker-compose.yml` file to run our infrastructure. 

### Why Docker?
Instead of installing heavy software directly on our Windows machine, Docker allows us to run software inside lightweight, isolated **Containers**. 
- Our `docker-compose.yml` spins up a **SQL Server** container (for our database) and a **RabbitMQ** container (a message broker so our microservices can talk to each other later).
- By using Docker, we guarantee that our local development environment matches the production server exactly.

## 3. Entity Framework Core (EF Core)

We installed EF Core into the `DefinitionService` to handle our database operations.

### What is EF Core?
EF Core is an Object-Relational Mapper (ORM). It acts as a translator between C# code and SQL. Instead of writing raw `CREATE TABLE` and `INSERT INTO` SQL statements, we write C# classes (`Models`), and EF Core handles the SQL for us.

### The Database Models
We used C# `record` types instead of `class` types for our models. Records are immutable (their data cannot be changed after creation) and are perfect for database mapping. We also use `Guid` (Globally Unique Identifiers) instead of integers for our IDs to prevent collisions in a distributed system.

1. **Workspace:** Represents a virtual office or a "Tenant" (e.g., a specific company's account). This ensures data isolation.
2. **WorkspaceRole:** An enum defining permission levels (Admin, Editor, Viewer).
3. **WorkspaceUser:** Links a specific user to a Workspace and gives them a Role.
4. **WorkflowDefinition:** Stores the actual visual graph created by the user on the React canvas. It belongs to a specific Workspace and saves the blueprint as a massive `GraphJson` string.

## 4. The DbContext and Dependency Injection

### The DbContext
The `DefinitionDbContext` class is the literal bridge between our C# models and the SQL database. We define `DbSet` properties inside it, which EF Core translates into actual SQL tables.

### Dependency Injection (DI)
In `Program.cs`, we registered our `DbContext` into the Dependency Injection container. 
We essentially told the application: *"Whenever a class asks for a database connection, use SQL Server, and connect using this specific Connection String located in `appsettings.json`."*

### Migrations
- `dotnet ef migrations add`: Scans our C# code and generates a blueprint (C# migration file) of how the database should look.
- `dotnet ef database update`: Connects to SQL Server running in Docker and physically executes the blueprint to build the tables.
