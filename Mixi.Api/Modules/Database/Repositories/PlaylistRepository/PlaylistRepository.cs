using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Music;
using Mixi.Shared.Models.Music;

namespace Mixi.Api.Modules.Database.Repositories.PlaylistRepository;

public class PlaylistRepository : IPlaylistRepository
{
    
    private readonly MSSQLMixiDbContext _context;
    public PlaylistRepository(MSSQLMixiDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<PlaylistDto>> GetAllPlaylists(string userId)
    {
        var playlists = await _context.Playlist
            .Where(p => p.UserId == userId)
            .Select(p => new PlaylistDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();
        return playlists;
    }

    public async Task<PlaylistDto> GetPlaylistById(string userId, int id)
    {
        var playlist = await _context.Playlist
            .Include(p => p.PlaylistItems)
            .Where(p => p.UserId == userId && p.Id == id)
            .Select(p => new PlaylistDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Items = p.PlaylistItems.Select(item => new PlaylistItemDto()
                {
                    Id = item.Id,
                    Source = item.SourceType,
                    SourceIdentifier = item.SourceIdentifier,
                    Title = item.Title,
                    Artist = item.Artist,
                    Album = item.Album,
                    AlbumArtUrl = item.ImageUrl,
                    Duration = item.Duration
                    
                }).ToList()
            })
            .FirstOrDefaultAsync();

        
        return playlist;
    }

    public async Task<PlaylistDto> CreatePlaylist(CreatePlaylistDto createPlaylistDto, string id)
    {
        var userId = id;
        var newPlaylist = new Playlist
        {
            Name = createPlaylistDto.Name,
            Description = createPlaylistDto.Description,
            UserId = userId,
            ImageUrl = createPlaylistDto.ImageUrl
        };
        
        _context.Playlist.Add(newPlaylist);
        
        await _context.SaveChangesAsync();

        var playlistDto = new PlaylistDto
        {
            Id = newPlaylist.Id,
            Name = newPlaylist.Name,
            Description = newPlaylist.Description,
            ImageUrl = newPlaylist.ImageUrl
        };
        
        return playlistDto;
    }

    public async Task DeletePlaylist(int id)
    {
        var playlist = await _context.Playlist.FindAsync(id);
        _context.Playlist.Remove(playlist);
        await _context.SaveChangesAsync();
    }
    

    public async Task<PlaylistItemDto?> CreatePlaylistItem(CreatePlaylistItemDto createPlaylistItemDto, int id, string userId)
    {
        var playlist = await _context.Playlist.FirstOrDefaultAsync(p => p.UserId == userId && p.Id == id);
        if (playlist == null)
        {
            return null;
        }

        var newTrack = new PlaylistItem
        {
            PlaylistId = id,
            SourceType = createPlaylistItemDto.SourceType,
            SourceIdentifier = createPlaylistItemDto.SourceIdentifier,
            Title = createPlaylistItemDto.Title,
            Artist = createPlaylistItemDto.Artist,
            Album = createPlaylistItemDto.Album,
            ImageUrl = createPlaylistItemDto.AlbumArtUrl,
            Duration = createPlaylistItemDto.Duration
        };
        
        _context.PlaylistItem.Add(newTrack);
        await _context.SaveChangesAsync();

        var trackDto = new PlaylistItemDto
        {
            Id = newTrack.Id,
            Source = newTrack.SourceType,
            SourceIdentifier = newTrack.SourceIdentifier,
            Title = newTrack.Title,
            Artist = newTrack.Artist,
            Album = newTrack.Album,
            AlbumArtUrl = newTrack.ImageUrl,
            Duration = newTrack.Duration
        };
        
        return trackDto;
    }
    
    public async Task DeletePlaylistItem(int playlistId, string userId, int trackId)
    {
        if (! await _context.Playlist.AnyAsync(p => p.Id == playlistId && p.UserId == userId))
        {
            return;
        }
            var track = await _context.PlaylistItem.FirstOrDefaultAsync(t => t.Id == trackId && t.PlaylistId == playlistId);
            
            if (track == null)
            {
                return;
            }
            
            _context.PlaylistItem.Remove(track);
            await _context.SaveChangesAsync();
    }
}

