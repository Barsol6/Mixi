namespace Mixi.Shared.Models.Music;

public class PlaylistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public List<PlaylistItemDto> Items { get; set; }
}