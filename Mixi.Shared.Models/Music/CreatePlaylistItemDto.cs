using System.Text.Json.Serialization;
using Mixi.Shared.Models.Enums;

namespace Mixi.Shared.Models.Music;

public class CreatePlaylistItemDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrackSource SourceType { get; set; }
    public string SourceIdentifier { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string AlbumArtUrl { get; set; }
    public double Duration { get; set; }    
}