using Mixi.Shared.Models.Enums;
using Mixi.Shared.Models.Music;
using SpotifyAPI.Web;
using YoutubeExplode;
using YoutubeExplode.Common;

namespace Mixi.Api.Modules.Music;

public class MetadataService
{
    private readonly IConfiguration _configuration;
    private SpotifyClient _spotifyClient;
    private readonly YoutubeClient _youtubeClient;
    public MetadataService(IConfiguration configuration)
    {
        _configuration = configuration;
        _youtubeClient = new YoutubeClient();
    }
    private async Task EnsureSpotifyClient()
    {
        if (_spotifyClient == null)
        {
            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
                
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new Exception("Spotify credentials are missing in configuration.");
            }

            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest(clientId, clientSecret);
            var response = await new OAuthClient(config).RequestToken(request);
            _spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));
        }
    }

    public async Task<CreatePlaylistItemDto> FetchMetadataAsync(string url)
    {
        var result = new CreatePlaylistItemDto();

        if (url.Contains("youtube.com") || url.Contains("youtu.be"))
        {
            try
            {
                var video = await _youtubeClient.Videos.GetAsync(url);

                result.SourceType = TrackSource.Youtube;
                result.SourceIdentifier = video.Id.Value;
                result.Title = video.Title;
                result.Artist = video.Author.ChannelTitle;
                result.Duration = video.Duration?.TotalMilliseconds ?? 0;
                result.AlbumArtUrl = video.Thumbnails.GetWithHighestResolution().Url;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e}");
            }
        }
        else if (url.Contains("spotify.com/track"))
        {
            try
            {
                await EnsureSpotifyClient();
                
                var trackId = url.Split("/track/")[1].Split("?")[0];
                var track = await _spotifyClient.Tracks.Get(trackId);
                
                result.SourceType = TrackSource.Spotify;
                result.SourceIdentifier = track.Uri;
                result.Title = track.Name;
                result.Artist = track.Artists.FirstOrDefault()?.Name ?? "Unknown";
                result.Album = track.Album.Name;
                result.Duration = track.DurationMs;
                result.AlbumArtUrl = track.Album.Images.FirstOrDefault()?.Url;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error{e}");
                throw;
            }
        }
        return result;
    }
}