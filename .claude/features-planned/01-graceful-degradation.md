# Feature: Graceful Degradation for All Components

## Goal
Complete requirement 3 from the change request: all Blazor components that inject optional services should degrade gracefully with clear in-page error messages instead of crashing.

## Context
`optional-scope-registries` already handled IScopeRegistry and ITenantRoleRegistry. Remaining:
- AuditLogView injects concrete `CompositeAuditLogger` — crashes if AddThargaAuditLogging() wasn't called
- Any component injecting IApiKeyAdministrationService when AddThargaApiKeys() wasn't called
- AddThargaTeamBlazor registers dependencies conditionally — ensure all paths are covered

## Acceptance Criteria
- Components render a clear error message when a required-but-optional service is missing
- No unhandled exceptions from missing DI registrations in any Platform Blazor component
- Tests verify both "service registered" and "service missing" paths
