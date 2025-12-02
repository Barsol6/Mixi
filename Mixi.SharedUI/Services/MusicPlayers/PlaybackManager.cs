using Mixi.Shared.Models.Music;

namespace Mixi.SharedUI.Services.MusicPlayers;

public class PlaybackManager
{
    private readonly IEnumerable<IMusicPlayer> _musicPlayers;

    private int _currentIndex;

    private List<PlaylistItemDto> _queue = new();

    public PlaybackManager(IEnumerable<IMusicPlayer> musicPlayers)
    {
        _musicPlayers = musicPlayers;

        foreach (var player in _musicPlayers) player.OnTrackFinished += PlayNext;
    }

    public PlaylistItemDto? CurrentTrack { get; private set; }

    public event Action OnTrackChanged;

    public async Task StartPlaylist(List<PlaylistItemDto> playlist)
    {
        _queue = playlist;
        _currentIndex = 0;
        await PlayCurrentTrack();
    }

    private async Task PlayCurrentTrack()
    {
        if (_currentIndex >= _queue.Count) return;

        var track = _queue[_currentIndex];

        CurrentTrack = track;

        OnTrackChanged?.Invoke();

        var player = _musicPlayers.FirstOrDefault(p => p.SupportedSource == track.SourceType);

        if (player != null)
        {
            foreach (var p in _musicPlayers) await p.StopAsync();

            Console.WriteLine($"[PlaybackManager] Playing {track.SourceIdentifier}");
            await player.PlayAsync(track.SourceIdentifier);
        }
    }
    
    public async Task PauseAsync()
    {
        if (CurrentTrack != null)
        {
      
            var player = _musicPlayers.FirstOrDefault(p => p.SupportedSource == CurrentTrack.SourceType);
            if (player != null)
            {
                await player.PauseAsync();
            }
        }
    }

    public async Task ResumeAsync()
    {
        if (CurrentTrack != null)
        {
            var player = _musicPlayers.FirstOrDefault(p => p.SupportedSource == CurrentTrack.SourceType);
            if (player != null)
            {
                await player.ResumeAsync();
            }
        }
    }


    public async void PlayNext()
    {
        _currentIndex++;
        await PlayCurrentTrack();
    }
    
    public async Task<double> GetCurrentPosition()
    {
        if (CurrentTrack == null) return 0;
        var player = _musicPlayers.FirstOrDefault(p => p.SupportedSource == CurrentTrack.SourceType);
        return player != null ? await player.GetCurrentPosition() : 0;
    }

    public async Task<double> GetTotalDuration()
    {
        if (CurrentTrack == null) return 0;
        var player =_musicPlayers.FirstOrDefault(p => p.SupportedSource == CurrentTrack.SourceType);
        return player != null ? await player.GetTotalDuration() : 0;
    }
}