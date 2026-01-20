using Microsoft.JSInterop;
using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class YoutubeMusicService : IMusicPlayer, IDisposable, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;

    private DotNetObjectReference<YoutubeMusicService>? _objectReference;

    public YoutubeMusicService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public TrackSource SupportedSource => TrackSource.Youtube;
    public event Action? OnTrackFinished;

    public async Task PlayAsync(string sourceIdentifier)
    {
        _objectReference = DotNetObjectReference.Create(this);
        
        var cleanId = ExtractVideoId(sourceIdentifier);

        await _jsRuntime.InvokeVoidAsync("initYouTubeApi");

        await _jsRuntime.InvokeVoidAsync("loadVideoYouTube", cleanId, _objectReference);
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
    
    public async Task PauseAsync()
    {
        await _jsRuntime.InvokeVoidAsync("pauseYouTube");
    }

    public async Task ResumeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("resumeYouTube");
    }
    public async Task<double> GetCurrentPosition()
    {
        try { return await _jsRuntime.InvokeAsync<double>("getYoutubePosition"); }
        catch { return 0; }
    }

    public async Task<double> GetTotalDuration()
    {
        try { return await _jsRuntime.InvokeAsync<double>("getYoutubeDuration"); }
        catch { return 0; }
    }

    private string ExtractVideoId(string url)
    {
        if(string.IsNullOrEmpty(url)) return string.Empty;

        if (url.Contains("youtube.com/watch?v="))
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["v"]??url;
        }

        if (url.Contains("youtube.be/"))
        {
            return url.Split('/').Last();
        }
        return url;
    }

    public void Dispose()
    {
        _objectReference?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("destroyYouTube");
        }
        catch(Exception e)
        {
            Console.WriteLine($"DisposeAsync Error: {e.Message}");
        }
        
        _objectReference?.Dispose();
    }
}