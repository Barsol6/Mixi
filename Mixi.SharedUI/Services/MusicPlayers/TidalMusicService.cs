using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class TidalMusicService : IMusicPlayer
{
    private readonly HttpClient _httpClient;
    
    public TrackSource SupportedSource => TrackSource.Tidal;
    public event Action? OnTrackFinished;
    
    public TidalMusicService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task PlayAsync(string sourceIdentifier)
    {
      //  string streamUrl = await GetTidalStreamUrl(sourceIdentifier);
        
        //Console.WriteLine($"[Tidal] Playing: {streamUrl} ");
    }
    
    public Task StopAsync()
    {
        Console.WriteLine("[Tidal] Stop");
        return Task.CompletedTask;
    }
    
    private async Task<string> GetTidalStreamUrl(string trackId)
    {
        return $"https://api.tidal.com/v1/tracks/{trackId}/streamUrl";
    }
}