using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mixi.Api.Modules.Users;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string UserType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}