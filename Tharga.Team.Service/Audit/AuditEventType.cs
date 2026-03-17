namespace Tharga.Team.Service.Audit;

public enum AuditEventType
{
    ServiceCall,
    AuthSuccess,
    AuthFailure,
    ScopeDenial,
    DataChange,
    RateLimit
}
