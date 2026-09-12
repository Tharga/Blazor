namespace Tharga.Blazor;

/// <summary>
/// Which form of a date <c>DateTimeView</c> shows as its text. The other form becomes the
/// tooltip, so both are always available.
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
    Absolute
}
