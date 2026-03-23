# Plan: Graceful Degradation

## Steps

- [x] 1. Write tests for AuditLogView graceful degradation (service missing + service present)
- [x] 2. Update AuditLogView to use IServiceProvider.GetService for CompositeAuditLogger
- [x] 3. Write tests for ApiKeyView graceful degradation (IApiKeyManagementService missing + present)
- [x] 4. Update ApiKeyView to use IServiceProvider.GetService for IApiKeyManagementService
- [x] 5. Run full test suite and verify build — 126 tests pass
- [~] 6. Commit
