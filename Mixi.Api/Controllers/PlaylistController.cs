using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Database;
using Mixi.Api.Modules.Database.Repositories.PlaylistRepository;
using Mixi.Api.Modules.Pdf;
using Mixi.Shared.Models.Music;

namespace Mixi.Api.Controllers;


[ApiController]
[Route("api/playlists")]
public class PlaylistController : ControllerBase
{
    
    
    private readonly IPlaylistRepository _playlistRepository;
    
    public PlaylistController(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    [HttpGet("{id}/getPlaylists")]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists(string userId)
    {
        
        var playlists = _playlistRepository.GetAllPlaylists(userId);
        
        return Ok(playlists);
    }
    
    [HttpPost("{userId}/{id}/getPlaylist")]
    public async Task<ActionResult<PlaylistDto>> GetPlaylist(int playlistId, string userId)  
    {
      

        var playlist = _playlistRepository.GetPlaylistById(userId, playlistId);
        
        return Ok(playlist);
    }

    [HttpPost("{id}/createPlaylist")]
    public async Task<ActionResult<PlaylistDto>> CreatePlaylist([FromBody] CreatePlaylistDto createPlaylistDto, string userId)
    {
      
        
        var newPlaylist = _playlistRepository.CreatePlaylist(createPlaylistDto, userId);;
        
        return CreatedAtAction(nameof(GetPlaylist), new {id = newPlaylist.Id}, newPlaylist);
    }
    
    [HttpDelete("{id}/deletePlaylist")]
    public async Task<ActionResult> DeletePlaylistbyid(int id)
    {
        await _playlistRepository.DeletePlaylist(id);
        
        return Ok();
    }

    [HttpPost("{id}/addTrackPlaylist")]
    public async Task<ActionResult<PlaylistItemDto>> AddTrackToPlaylist(int id, string userId,
        [FromBody] CreatePlaylistItemDto playlistItemDto)
    {
        var newPlaylistItemDto = await _playlistRepository.CreatePlaylistItem(playlistItemDto, id, userId);
        
        return Ok(newPlaylistItemDto);
    }

    [HttpDelete("{playlistId}/deleteTrackPlaylist")]
    public async Task<IActionResult> DeleteTrackFromPlaylist(int playlistId, string userId, int trackId)
    {
        await _playlistRepository.DeletePlaylistItem(playlistId, userId, trackId);
        
        return Ok();
    }
    

    
}