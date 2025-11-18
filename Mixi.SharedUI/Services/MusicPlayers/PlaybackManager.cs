using Mixi.Shared.Models.Music;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class PlaybackManager
{
    public PlaylistItemDto CurrentTrack { get; private set; }
    
    public event Action OnTrackChanged;
    
    private readonly IEnumerable<IMusicPlayer> _musicPlayers;
    
    private List<PlaylistItemDto> _queue = new();
    
    private int _currentIndex = 0;
    
    public PlaybackManager(IEnumerable<IMusicPlayer> musicPlayers)
    {
        _musicPlayers = musicPlayers;

        foreach (var player in _musicPlayers)
        {
            player.OnTrackFinished += PlayNext;
        }
    }

    public async Task StartPlaylist(List<PlaylistItemDto> playlist)
    {
        _queue = playlist;
        _currentIndex = 0;
        await PlayCurrentTrack();
    }

    private async Task PlayCurrentTrack()
    {
        if (_currentIndex >= _queue.Count)
        {
            return;
        }
        
        var track = _queue[_currentIndex];
        
        CurrentTrack = track;
        
        OnTrackChanged?.Invoke();

        var player = _musicPlayers.FirstOrDefault(p => p.SupportedSource == track.SourceType);

        if (player!=null)
        {
            foreach (var p in _musicPlayers)
            {
                await p.StopAsync();
            }
            
            Console.WriteLine($"[PlaybackManager] Playing {track.SourceIdentifier}");
            await player.PlayAsync(track.SourceIdentifier);
        }

    }
    
    
    public async void PlayNext()
    {
        _currentIndex++;
        await PlayCurrentTrack();
    }
}