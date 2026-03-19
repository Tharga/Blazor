# Tharga Team MongoDB
[![NuGet](https://img.shields.io/nuget/v/Tharga.Team.MongoDB)](https://www.nuget.org/packages/Tharga.Team.MongoDB)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Team.MongoDB)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

MongoDB persistence layer for [Tharga.Team](https://www.nuget.org/packages/Tharga.Team). Provides repository base classes for storing teams and users in MongoDB.

## What's included

- `TeamRepository` / `ITeamRepository` - MongoDB-backed team storage.
- `UserRepository` / `IUserRepository` - MongoDB-backed user storage.
- `TeamRepositoryCollection` / `UserRepositoryCollection` - MongoDB collection definitions with indexes.
- `TeamMemberBase` - Base record for team member entities.
- `TeamEntityBase` - Base record for team entities.
- `UserServiceRepositoryBase` - User service base class with MongoDB persistence.

## Dependencies

- [Tharga.Team](https://www.nuget.org/packages/Tharga.Team) - Domain models and service abstractions.
- [Tharga.MongoDB](https://www.nuget.org/packages/Tharga.MongoDB) - MongoDB repository infrastructure.

## Related packages

| Package | Description |
|---------|-------------|
| [Tharga.Blazor](https://www.nuget.org/packages/Tharga.Blazor) | UI components for Blazor Server and WebAssembly |
| [Tharga.Team](https://www.nuget.org/packages/Tharga.Team) | Domain models and authorization primitives |
| [Tharga.Api](https://www.nuget.org/packages/Tharga.Api) | Server-only API key auth handler, Swagger, audit logging |
