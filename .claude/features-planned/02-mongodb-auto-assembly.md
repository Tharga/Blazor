# Feature: MongoDB Auto-Assembly Registration

## Goal
Complete requirement 4: Platform packages register their own assemblies for MongoDB auto-scanning so consumers with different name prefixes (e.g. Quilt4Net, PlutusWave) don't need to manually call AddAutoRegistrationAssembly.

## Context
The MongoDB assembly scanner in Tharga.Runtime uses the entry assembly's first namespace component as a filter. When the consumer's assembly name doesn't start with "Tharga", all Platform MongoDB types are silently missed. The workaround exists (AddAutoRegistrationAssembly) but is undocumented and non-obvious.

## Approach
- AddThargaTeamRepository() should call AddAutoRegistrationAssembly for Tharga.Team.MongoDB assembly
- AddThargaApiKeys() should ensure its repository types are discoverable
- This is a Platform-side fix — no changes needed in Tharga.MongoDB itself

## Acceptance Criteria
- A consumer with a non-Tharga entry assembly name gets Platform MongoDB types auto-registered
- Existing consumers are unaffected (no breaking change)
- Tests verify assembly registration occurs
