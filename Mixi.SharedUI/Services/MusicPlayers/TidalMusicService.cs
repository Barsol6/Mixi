using Microsoft.JSInterop;
using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class TidalMusicService : IMusicPlayer, IDisposable
{
    private readonly HttpClient _httpClient;
    private DotNetObjectReference<TidalMusicService>? _objectReference;
    private readonly IJSRuntime _jsRuntime;

    public TrackSource SupportedSource => TrackSource.Tidal;
    public event Action? OnTrackFinished;

    public TidalMusicService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }
    

    public async Task PlayAsync(string sourceIdentifier)
    {
            string streamUrl = await GetTidalStreamUrl(sourceIdentifier);
            
            _objectReference ??= DotNetObjectReference.Create(this);
            
            await _jsRuntime.InvokeVoidAsync("howlerPlayer.play", streamUrl, _objectReference);
            
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

    private async Task<string> GetTidalStreamUrl(string trackId)
    {
        return $"https://api.tidal.com/v1/tracks/{trackId}/streamUrl";
    }
    
    [JSInvokable]
    public void OnTrackEnded()
    {
        OnTrackFinished?.Invoke();
    }

    public void Dispose()
    {
        _objectReference?.Dispose();
    }
    
    public async Task<double> GetCurrentPosition()
    {
        try { return await _jsRuntime.InvokeAsync<double>("howlerPlayer.getPosition"); }
        catch { return 0; }
    }

    public async Task<double> GetTotalDuration()
    {
        try { return await _jsRuntime.InvokeAsync<double>("howlerPlayer.getDuration"); }
        catch { return 0; }
    }
}