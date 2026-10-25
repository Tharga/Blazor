using Bunit;
using Microsoft.JSInterop;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor.Tests;

public class BrowserLocalStorageTests
{
    private const string Key = "ExpandableCard.someCard.Selected";

    private readonly FakeBrowserStorage _jsRuntime = new();
    private readonly BrowserLocalStorage _sut;

    public BrowserLocalStorageTests()
    {
        _sut = new BrowserLocalStorage(_jsRuntime);
    }

    [Fact]
    public void Constructor_WithoutJsRuntime_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new BrowserLocalStorage(null!));
    }

    [Fact]
    public async Task GetItemAsync_ReturnsTheRawStoredString()
    {
        _jsRuntime.SeedRaw(Key, "raw value");

        var value = await _sut.GetItemAsync(Key);

        Assert.Equal("raw value", value);
        Assert.Equal(Key, Assert.Single(_jsRuntime.Reads));
    }

    [Fact]
    public async Task GetItemAsync_NothingStored_ReturnsNull()
    {
        var value = await _sut.GetItemAsync(Key);

        Assert.Null(value);
    }

    [Fact]
    public async Task SetItemAsync_StoresTheRawString()
    {
        await _sut.SetItemAsync(Key, "raw value");

        Assert.Equal("raw value", _jsRuntime.Raw(Key));
        Assert.Equal(Key, Assert.Single(_jsRuntime.Writes));
    }

    [Fact]
    public async Task RemoveItemAsync_RemovesTheEntry()
    {
        _jsRuntime.SeedRaw(Key, "true");

        await _sut.RemoveItemAsync(Key);

        Assert.Null(_jsRuntime.Raw(Key));
        Assert.Equal(Key, Assert.Single(_jsRuntime.Removals));
    }

    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public async Task SetAsync_StoresJson(bool value, string expected)
    {
        await _sut.SetAsync<bool?>(Key, value);

        Assert.Equal(expected, _jsRuntime.Raw(Key));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public async Task GetAsync_ReadsJson(string stored, bool expected)
    {
        _jsRuntime.SeedRaw(Key, stored);

        var value = await _sut.GetAsync<bool?>(Key);

        Assert.Equal(expected, value);
    }

    [Fact]
    public async Task GetAsync_NothingStored_ReturnsDefault()
    {
        var value = await _sut.GetAsync<bool?>(Key);

        Assert.Null(value);
    }

    [Fact]
    public async Task GetAsync_InvalidJson_ReturnsDefault()
    {
        _jsRuntime.SeedRaw(Key, "not json");

        var value = await _sut.GetAsync<bool?>(Key);

        Assert.Null(value);
    }

    [Fact]
    public async Task SetAsyncThenGetAsync_RoundTrips()
    {
        await _sut.SetAsync(Key, new[] { "a", "b" });

        var value = await _sut.GetAsync<string[]>(Key);

        Assert.Equal(["a", "b"], value);
    }

    [Fact]
    public async Task GetItemAsync_JsFailure_IsPassedToTheCaller()
    {
        var sut = new BrowserLocalStorage(new FakeBrowserStorage { Failure = new JSException("blocked") });

        await Assert.ThrowsAsync<JSException>(async () => await sut.GetItemAsync(Key));
    }

    public static TheoryData<Exception> UnavailableExceptions =>
    [
        new JSException("blocked"),
        new JSDisconnectedException("disconnected"),
        new InvalidOperationException("JavaScript interop calls cannot be issued at this time."),
        new TaskCanceledException()
    ];

    [Theory]
    [MemberData(nameof(UnavailableExceptions))]
    public void IsUnavailable_StorageFailures_AreRecognized(Exception exception)
    {
        Assert.True(BrowserLocalStorage.IsUnavailable(exception));
    }

    [Fact]
    public void IsUnavailable_OtherExceptions_AreNot()
    {
        Assert.False(BrowserLocalStorage.IsUnavailable(new ArgumentException("bug")));
    }
}

public class BrowserLocalStorageInteropTests : BunitContext
{
    private const string Key = "ExpandableCard.someCard.Selected";

    public BrowserLocalStorageInteropTests()
    {
        JSInterop.Mode = JSRuntimeMode.Strict;
    }

    [Fact]
    public async Task GetAsync_CallsLocalStorageGetItemWithTheKey()
    {
        JSInterop.Setup<string>("localStorage.getItem", Key).SetResult("true");
        var sut = new BrowserLocalStorage(JSInterop.JSRuntime);

        var value = await sut.GetAsync<bool?>(Key);

        Assert.True(value);
    }

    [Fact]
    public async Task SetAsync_CallsLocalStorageSetItemWithTheKeyAndJson()
    {
        var setItem = JSInterop.SetupVoid("localStorage.setItem", Key, "false");
        setItem.SetVoidResult();
        var sut = new BrowserLocalStorage(JSInterop.JSRuntime);

        await sut.SetAsync<bool?>(Key, false);

        setItem.VerifyInvoke("localStorage.setItem");
    }

    [Fact]
    public async Task RemoveItemAsync_CallsLocalStorageRemoveItemWithTheKey()
    {
        var removeItem = JSInterop.SetupVoid("localStorage.removeItem", Key);
        removeItem.SetVoidResult();
        var sut = new BrowserLocalStorage(JSInterop.JSRuntime);

        await sut.RemoveItemAsync(Key);

        removeItem.VerifyInvoke("localStorage.removeItem");
    }
}
