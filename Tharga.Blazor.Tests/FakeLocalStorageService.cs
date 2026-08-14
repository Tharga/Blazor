using System.Text.Json;
using Blazored.LocalStorage;

namespace Tharga.Blazor.Tests;

internal class FakeLocalStorageService : ILocalStorageService
{
    private readonly Dictionary<string, string> _store = new();
    private readonly List<string> _reads = [];
    private readonly List<string> _writes = [];

    public IReadOnlyList<string> Reads => _reads;

    public IReadOnlyList<string> Writes => _writes;

    public IReadOnlyCollection<string> Keys => _store.Keys;

    public Exception? Failure { get; init; }

    public void Seed<T>(string key, T value)
    {
        _store[key] = JsonSerializer.Serialize(value);
    }

    public T? Read<T>(string key)
    {
        return _store.TryGetValue(key, out var json) ? JsonSerializer.Deserialize<T>(json) : default;
    }

    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        _reads.Add(key);
        if (Failure != null) throw Failure;

        return Read<T>(key);
    }

    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        _writes.Add(key);
        if (Failure != null) throw Failure;

        Seed(key, data);
    }

    public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        _reads.Add(key);

        return _store.GetValueOrDefault(key);
    }

    public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        _writes.Add(key);
        _store[key] = data;
    }

    public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default) => ValueTask.FromResult<IEnumerable<string>>(_store.Keys);

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default) => ValueTask.FromResult(_store.ContainsKey(key));

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default) => ValueTask.FromResult(_store.Count);

    public ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default) => ValueTask.FromResult<string?>(_store.Keys.ElementAt(index));

    public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        _store.Remove(key);
        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        foreach (var key in keys)
        {
            _store.Remove(key);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        _store.Clear();
        return ValueTask.CompletedTask;
    }

    public event EventHandler<ChangingEventArgs>? Changing
    {
        add { }
        remove { }
    }

    public event EventHandler<ChangedEventArgs>? Changed
    {
        add { }
        remove { }
    }
}
