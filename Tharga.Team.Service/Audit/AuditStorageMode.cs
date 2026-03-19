namespace Tharga.Team.Service.Audit;

[Flags]
public enum AuditStorageMode
{
    None = 0,
    Logger = 1,
    MongoDB = 2
}
