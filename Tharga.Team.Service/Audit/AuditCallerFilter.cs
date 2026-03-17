namespace Tharga.Team.Service.Audit;

[Flags]
public enum AuditCallerFilter
{
    None = 0,
    Api = 1,
    Web = 2
}
