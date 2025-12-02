using Microsoft.JSInterop;
using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class LocalMusicService : IMusicPlayer, IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private DotNetObjectReference<LocalMusicService>? _objectReference;

    public LocalMusicService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void Dispose()
    {
        _objectReference?.Dispose();
    }

    public TrackSource SupportedSource => TrackSource.LocalFile;
    public event Action? OnTrackFinished;

    public async Task PlayAsync(string sourceIdentifier)
    {
        _objectReference = DotNetObjectReference.Create(this);
        await _jsRuntime.InvokeVoidAsync("howlerPlayer.play", sourceIdentifier, _objectReference);
    }

    public async Task StopAsync()
    {
        await _jsRuntime.InvokeVoidAsync("howlerPlayer.stop");
    }

    public async Task PauseAsync()
    {
        await _jsRuntime.InvokeVoidAsync("howlerPlayer.pause");
    }

    public async Task ResumeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("howlerPlayer.resume");
    }

    [JSInvokable]
    public void OnTrackEnded()
    {
        Console.WriteLine("[Local] Track ended via JS Interop");
        OnTrackFinished?.Invoke();
    }

    public async Task<double> GetCurrentPosition()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<double>("howlerPlayer.getPosition");
        }
        catch
        {
            return 0;
        }
    }

    public async Task<double> GetTotalDuration()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<double>("howlerPlayer.getDuration");
        }
        catch
        {
            return 0;
        }
    }
    
}