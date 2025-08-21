using Microsoft.JSInterop;
using Streckenbuch.Components.Models;
using Streckenbuch.Components.Services;
using System.Collections.Concurrent;

namespace Streckenbuch.Client.Services;

public class ScreenWakeLockService : IScreenWakeLockService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ConcurrentDictionary<int, WakeLockSentinel> _wakeLocks;
    private int _nextId;

    public ScreenWakeLockService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _wakeLocks = new ConcurrentDictionary<int, WakeLockSentinel>();
        _nextId = 0;
    }

    public async Task<WakeLockSentinel> RequestWakeLockAsync()
    {
        // Check if the browser supports the screen wake lock API
        var isSupported = await IsSupportedAsync();
        if (!isSupported)
        {
            throw new NotSupportedException("The browser does not support the screen wake lock API.");
        }

        // Request a screen wake lock and get a JS object reference
        var jsObjectReference = await _jsRuntime.InvokeAsync<IJSObjectReference>("navigator.wakeLock.request", "screen");

        // Create a sentinel object and store it in a dictionary
        var id = Interlocked.Increment(ref _nextId);
        var sentinel = new WakeLockSentinel(id, jsObjectReference);
        _wakeLocks.TryAdd(id, sentinel);

        // Return the sentinel object
        return sentinel;
    }

    public async Task ReleaseWakeLockAsync(WakeLockSentinel sentinel)
    {
        // Check if the sentinel object is valid
        if (sentinel == null || sentinel.JsObjectReference == null)
        {
            throw new ArgumentNullException(nameof(sentinel));
        }

        // Release the screen wake lock and dispose the JS object reference
        await sentinel.JsObjectReference.InvokeVoidAsync("release");
        await sentinel.JsObjectReference.DisposeAsync();

        // Remove the sentinel object from the dictionary
        _wakeLocks.TryRemove(sentinel.Id, out _);
    }

    public async Task<bool> IsSupportedAsync()
    {
        // Check if the navigator.wakeLock property exists
        return await _jsRuntime.InvokeAsync<bool>("eval", "typeof navigator.wakeLock !== 'undefined'");
    }
}