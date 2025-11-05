using Mixi.Shared.Models.Enums;

namespace Mixi.Api.Modules.Music;

public class PlaylistItem
{
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    
    public virtual Playlist Playlist { get; set; }
    
    public TrackSource SourceType { get; set; }
    
    public string SourceIdentifier { get; set; }
    
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string? ImageUrl { get; set; }
    public double Duration { get; set; }
    
    
    
}