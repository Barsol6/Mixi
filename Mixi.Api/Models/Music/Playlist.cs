namespace Mixi.Api.Modules.Music;

public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public string UserId { get; set; }
    
    public virtual ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
}