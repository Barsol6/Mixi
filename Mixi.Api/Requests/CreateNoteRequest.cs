using System.ComponentModel.DataAnnotations;

namespace Mixi.Api.Requests;

public class CreateNoteRequest
{
    [Required] public string NoteName { get; set; }
}