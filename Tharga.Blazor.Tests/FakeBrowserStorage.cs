using System.Text.Json;
using Microsoft.JSInterop;

namespace Tharga.Blazor.Tests;

internal class FakeBrowserStorage : IJSRuntime
{
    private const string GetItem = "localStorage.getItem";
    private const string SetItem = "localStorage.setItem";
    private const string RemoveItem = "localStorage.removeItem";

    private readonly Dictionary<string, string> _store = new();
    private readonly List<string> _reads = [];
    private readonly List<string> _writes = [];
    private readonly List<string> _removals = [];

    public IReadOnlyList<string> Reads => _reads;

    public IReadOnlyList<string> Writes => _writes;

    public IReadOnlyList<string> Removals => _removals;

    public IReadOnlyCollection<string> Keys => _store.Keys;

    public Exception? Failure { get; init; }

    public void Seed<T>(string key, T value)
    {
        _store[key] = JsonSerializer.Serialize(value);
    }

    public void SeedRaw(string key, string value)
    {
        _store[key] = value;
    }

    public string? Raw(string key) => _store.GetValueOrDefault(key);

    public T? Read<T>(string key)
    {
        return _store.TryGetValue(key, out var json) ? JsonSerializer.Deserialize<T>(json) : default;
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
    {
        return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
    }

    public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        await Task.Yield();

        var key = (string)args![0]!;
        switch (identifier)
        {
            case GetItem:
                _reads.Add(key);
                if (Failure != null) throw Failure;
                return (TValue)(object?)_store.GetValueOrDefault(key)!;
            case SetItem:
                _writes.Add(key);
                if (Failure != null) throw Failure;
                _store[key] = (string)args[1]!;
                return default!;
            case RemoveItem:
                _removals.Add(key);
                if (Failure != null) throw Failure;
                _store.Remove(key);
                return default!;
            default:
                throw new InvalidOperationException($"Unexpected JavaScript call '{identifier}'.");
        }
    }
}
