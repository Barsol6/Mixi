using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mixi.Api.Modules.Database;
using Mixi.Api.Modules.Database.Repositories.PlaylistRepository;
using Mixi.Api.Modules.Pdf;
using Mixi.Shared.Models.Music;

namespace Mixi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/playlists")]
public class PlaylistController : ControllerBase
{
    
    private string? GetUserName()
    {
        return User.FindFirst(ClaimTypes.Name)?.Value;
    }
    
    private readonly IPlaylistRepository _playlistRepository;
    
    
    public PlaylistController(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    [HttpGet("getPlaylists")]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
    {
        
        var userId = GetUserName();

        if (userId == null)
        {
            return Unauthorized();
        }
        
        var playlists = await _playlistRepository.GetAllPlaylists(userId);
        
        return Ok(playlists);
    }
    
    [HttpGet("{id}/getPlaylist")]
    public async Task<ActionResult<PlaylistDto>> GetPlaylist(int id)
    {

        var playlistId = id;
        var userId = GetUserName();

        if (userId == null)
        {
            return Unauthorized();
        }

        var playlist = await  _playlistRepository.GetPlaylistById(userId, playlistId);
        
        return Ok(playlist);
    }

    [HttpPost("createPlaylist")]
    public async Task<ActionResult<PlaylistDto>> CreatePlaylist([FromBody] CreatePlaylistDto createPlaylistDto)
    {
      var userId = GetUserName();

      if (userId == null)
      {
          return Unauthorized();
      }
        
        var newPlaylist = await  _playlistRepository.CreatePlaylist(createPlaylistDto, userId);;
        
        return CreatedAtAction(nameof(GetPlaylist), new {id = newPlaylist.Id}, newPlaylist);
    }
    
    [HttpDelete("{id}/deletePlaylist")]
    public async Task<ActionResult> DeletePlaylistbyid(int id)
    {
        
        var userId = GetUserName();

        if (userId == null)
        {
            return Unauthorized();
        }
        
        await _playlistRepository.DeletePlaylist(id, userId);;
        
        return Ok();
    }

    [HttpPost("{id}/addTrackPlaylist")]
    public async Task<ActionResult<PlaylistItemDto>> AddTrackToPlaylist(int id, 
        [FromBody] CreatePlaylistItemDto playlistItemDto)
    {
        var userId = GetUserName();

        if (userId == null)
        {
            return Unauthorized();
        }
        
        var newPlaylistItemDto = await _playlistRepository.CreatePlaylistItem(playlistItemDto, id, userId);

        if (newPlaylistItemDto == null)
        {
            return  NotFound("Playlist not found");
        }
        
        return Ok(newPlaylistItemDto);
    }

    [HttpDelete("{playlistId}/{trackId}/deleteTrackFromPlaylist")]
    public async Task<IActionResult> DeleteTrackFromPlaylist(int playlistId, int trackId)
    {
        
        var userId = GetUserName();
        
        if (userId == null)
        {
            return Unauthorized();
        }
        
        await _playlistRepository.DeletePlaylistItem(playlistId, userId, trackId);
        
        return NoContent();
    }
    
    

    
}