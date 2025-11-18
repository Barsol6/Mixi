using Microsoft.JSInterop;
using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class YoutubeMusicService : IMusicPlayer
{
    private readonly IJSRuntime _jsRuntime;
    
    private DotNetObjectReference<YoutubeMusicService>? _objectReference;
    
    public TrackSource SupportedSource => TrackSource.Youtube;
    public event Action? OnTrackFinished;
    
    public YoutubeMusicService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task PlayAsync(string sourceIdentifier)
    {
        
        _objectReference = DotNetObjectReference.Create(this);

        await _jsRuntime.InvokeVoidAsync("initYouTubeApi");

        await _jsRuntime.InvokeVoidAsync("loadVideoYouTube",sourceIdentifier, _objectReference);
    }
    
    public async Task StopAsync()
    {
        await _jsRuntime.InvokeVoidAsync("stopYouTube");
    }
    
    [JSInvokable]
    public void OnTrackEnded()
    {
        OnTrackFinished?.Invoke();
    }
}