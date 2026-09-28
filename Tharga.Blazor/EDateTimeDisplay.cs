namespace Tharga.Blazor;

/// <summary>
/// Which form of a date <c>DateTimeView</c> shows as its text. Whatever the text leaves out becomes
/// the tooltip, so every form is always available.
/// </summary>
public enum EDateTimeDisplay
{
    /// <summary>
    /// How long ago it was — "34 seconds ago". Reads well, but describes a moving quantity, so it
    /// goes stale unless <c>DateTimeView.RefreshInterval</c> is set.
    /// </summary>
    Relative,

    /// <summary>
    /// The timestamp itself — "2026-09-12 07:54:35". Cannot go stale, and two events a moment
    /// apart stay distinguishable. Prefer it wherever the exact ordering of near-simultaneous
    /// entries matters, such as an audit log.
    /// </summary>
    Absolute,

    /// <summary>
    /// The day alone — "2026-09-12". Narrow enough for a grid column and cannot go stale. The
    /// tooltip carries both the full timestamp and how long ago it was.
    /// </summary>
    Date
}
