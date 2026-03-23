# Feature: Single Top-Level AddThargaPlatform Registration

## Goal
Complete requirement 1: provide a single entry point (AddThargaPlatform) that sets up all core services with sensible defaults, reducing the 7-call registration to 1.

## Context
Currently consumers need: AddThargaAuth, AddThargaApiKeyAuthentication, AddThargaApiKeys, AddThargaTeamBlazor, AddThargaTeamRepository, AddThargaScopes, AddThargaTenantRoles. Missing any one causes a runtime crash.

## Approach
- Create AddThargaPlatform() extension method that orchestrates all registrations with default options
- Accept a single options object (ThargaPlatformOptions) that exposes sub-options for each subsystem
- Support appsettings.json binding with code-override-wins pattern
- Individual Add* methods remain available for advanced/partial use
- Decide which project hosts this (likely a new Tharga.Platform package or Tharga.Team.Blazor)

## Acceptance Criteria
- A consumer can call one method and get a working Platform setup
- All sub-options are configurable through the single options object
- Individual Add* methods still work independently
- Tests verify the combined registration works end-to-end
