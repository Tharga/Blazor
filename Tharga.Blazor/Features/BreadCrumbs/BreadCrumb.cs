namespace Tharga.Blazor.Features.BreadCrumbs;

public record BreadCrumb
{
    public required string Text { get; init; }
    public string Path { get; init; }
}