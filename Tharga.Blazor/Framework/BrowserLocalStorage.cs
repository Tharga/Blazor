using System.Text.Json;
using Microsoft.JSInterop;

namespace Tharga.Blazor.Framework;

/// <summary>
/// A minimal wrapper over the browser's <c>window.localStorage</c>, called through <see cref="IJSRuntime"/>.
/// Values are stored as System.Text.Json, the format Blazored.LocalStorage wrote, so state saved by
/// earlier versions reads back unchanged.
/// </summary>
internal sealed class BrowserLocalStorage
{
    internal const string GetItemIdentifier = "localStorage.getItem";
    internal const string SetItemIdentifier = "localStorage.setItem";
    internal const string RemoveItemIdentifier = "localStorage.removeItem";

    private readonly IJSRuntime _jsRuntime;

    public BrowserLocalStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public ValueTask<string> GetItemAsync(string key) => _jsRuntime.InvokeAsync<string>(GetItemIdentifier, key);

    public ValueTask SetItemAsync(string key, string value) => _jsRuntime.InvokeVoidAsync(SetItemIdentifier, key, value);

    public ValueTask RemoveItemAsync(string key) => _jsRuntime.InvokeVoidAsync(RemoveItemIdentifier, key);

    /// <summary>
    /// Returns <c>default</c> when nothing is stored, or when the stored text is not valid JSON for <typeparamref name="T"/>.
    /// </summary>
    public async ValueTask<T> GetAsync<T>(string key)
    {
        var json = await GetItemAsync(key);
        if (string.IsNullOrEmpty(json)) return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public ValueTask SetAsync<T>(string key, T value) => SetItemAsync(key, JsonSerializer.Serialize(value));

    /// <summary>
    /// Whether <paramref name="exception"/> means the browser's storage could not be reached: JavaScript
    /// interop is unavailable during prerendering, gone after a circuit disconnects, and storage can be
    /// blocked by the browser.
    /// </summary>
    public static bool IsUnavailable(Exception exception) => exception is JSException or JSDisconnectedException or InvalidOperationException or TaskCanceledException;
}
