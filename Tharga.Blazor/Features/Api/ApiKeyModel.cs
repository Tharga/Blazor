namespace Tharga.Blazor.Features.Api;

public record ApiKeyModel
{
    public required string Key { get; init; }
    public required string Name { get; init; }
    public required string ApiKey { get; init; }
    public string VisibleKey { get; set; }
}