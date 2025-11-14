using System.Net.Http.Json;
using Mixi.Shared.Models.Music;

namespace Mixi.App.Services.PlaylistServices;

public class PlaylistApiService
{
    private readonly HttpClient _httpClient;
    
    public PlaylistApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PlaylistDto>?> GetMyPlaylistsAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<List<PlaylistDto>>($"api/playlists/{userId}/getPlaylists");
    }
    
  

}