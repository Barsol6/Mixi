using Mixi.Shared.Models.Enums;

namespace Mixi.SharedUI.Services.MusicPlayers;

public interface IMusicPlayer
{
    TrackSource SupportedSource { get; }
    Task PlayAsync(string sourceIdentifier);
    Task StopAsync();
    Task PauseAsync();
    Task ResumeAsync();
    event Action OnTrackFinished;
    Task<double> GetCurrentPosition();
    Task<double> GetTotalDuration();
}