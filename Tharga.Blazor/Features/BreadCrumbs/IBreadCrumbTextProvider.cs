namespace Tharga.Blazor.Features.BreadCrumbs;

/// <summary>
/// Supplies the text for breadcrumb segments that come from the URL, for example to translate them.
/// Register it as a scoped or singleton service. It is asked every time the breadcrumbs are read, so it can follow
/// the current language; call <see cref="BreadCrumbService.Refresh"/> to re-render after a change.
/// </summary>
public interface IBreadCrumbTextProvider
{
    /// <param name="segment">The URL segment, as it appears in the URL (e.g. <c>cases</c>).</param>
    /// <param name="path">The route path up to and including the segment, without a leading slash (e.g. <c>cases/abc</c>).</param>
    /// <returns>The text to show, or null to show the segment's default capitalised text.</returns>
    string GetText(string segment, string path);
}
