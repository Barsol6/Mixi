using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public interface IMusicPlayer
{
    TrackSource SupportedSource { get; }
    Task PlayAsync(string sourceIdentifier);
    Task StopAsync();
    event Action OnTrackFinished;
}