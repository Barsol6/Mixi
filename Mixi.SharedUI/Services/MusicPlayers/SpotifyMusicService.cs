using Microsoft.JSInterop;
using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class SpotifyMusicService : IMusicPlayer
{
    private readonly IJSRuntime _jsRuntime;

    private DotNetObjectReference<SpotifyMusicService>? _objectReference;

    public SpotifyMusicService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public TrackSource SupportedSource => TrackSource.Spotify;
    public event Action? OnTrackFinished;

    public async Task PlayAsync(string sourceIdentifier)
    {
        if (_objectReference == null) _objectReference = DotNetObjectReference.Create(this);


        await _jsRuntime.InvokeVoidAsync("initSpotifyPlayer", "YOUR_ACCESS_TOKEN_TO_BE_IMPLEMENTED", _objectReference);

        await _jsRuntime.InvokeVoidAsync("playSpotifyTrack", sourceIdentifier);
    }

    public async Task StopAsync()
    {
        await _jsRuntime.InvokeVoidAsync("pauseSpotify");
    }

    [JSInvokable]
    public void OnTrackEnded()
    {
        Console.WriteLine("[Spotify] Track ended via JS Interop");
        OnTrackFinished?.Invoke();
    }
    
    public async Task PauseAsync()
    {
        await _jsRuntime.InvokeVoidAsync("pauseSpotify");
    }

    public async Task ResumeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("resumeSpotify");
    }

    public async Task<double> GetCurrentPosition()
    {
        return 0;//To implement
    }

    public async Task<double> GetTotalDuration()
    {
        return 0;//To implement
    }
    
}