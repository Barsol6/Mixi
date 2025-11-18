using Microsoft.JSInterop;
using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class SpotifyMusicService : IMusicPlayer
{
    private readonly JSRuntime _jsRuntime;
    
    private DotNetObjectReference<SpotifyMusicService>? _objectReference;
    
    public TrackSource SupportedSource => TrackSource.Spotify;
    public event Action? OnTrackFinished;
    
    public SpotifyMusicService(JSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task PlayAsync(string sourceIdentifier)
    {
        if (_objectReference == null)
        {
            _objectReference = DotNetObjectReference.Create(this);   
        }


        await _jsRuntime.InvokeVoidAsync("initSpotifyPlayer", "YOUR_ACCESS_TOKEN_TO_BE_IMPLEMENTED", _objectReference);
        
        await _jsRuntime.InvokeVoidAsync("playSpotifyTrack", sourceIdentifier);
    }

    public async Task StopAsync()
    {
        await _jsRuntime.InvokeVoidAsync("window.spotifyPlayer.pause");
    }
    
    [JSInvokable]
    public void OnTrackEnded()
    {
        Console.WriteLine("[Spotify] Track ended via JS Interop");
        OnTrackFinished?.Invoke();
    }
}