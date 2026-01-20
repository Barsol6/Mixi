using Mixi.Shared.Models.Music;

namespace Mixi.Api.Modules.Database.Repositories.PlaylistRepository;

public interface IPlaylistRepository
{
    public Task<IEnumerable<PlaylistDto>> GetAllPlaylists(string userId);
    public Task<PlaylistDto> GetPlaylistById(string userId, int id);

    public Task<PlaylistDto> CreatePlaylist(CreatePlaylistDto playlistDto, string userId);

    public Task DeletePlaylist(int id, string userId);

    public Task<PlaylistItemDto?>
        CreatePlaylistItem(CreatePlaylistItemDto createPlaylistItemDto, int id, string userId);

    public Task DeletePlaylistItem(int playlistId, string userId, int trackId);
}