using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mixi.Api.Modules.Account;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Users;
using Mixi.Shared.Models.Account;

namespace Mixi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly PasswordHash _passwordHash;
    private readonly IUserRepository _userRepository;


    public UserController(IUserRepository userRepository, PasswordHash passwordHash, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHash = passwordHash;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Account account)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (_userRepository.CheckIfExists(account.Username))
        {
            Console.WriteLine("dupa");
            return Conflict(new { message = "Account already exists" });
        }

        var hashedPassword = _passwordHash.HashPasswords(account.Password, account.Username);

        var newUser = new User
        {
            Username = account.Username,
            Password = hashedPassword,
            UserType = account.UserType
        };

        await _userRepository.AddUserAsync(newUser);

        return Ok(new { message = "Account created" });
        ;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Account account)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_userRepository.CheckIfExists(account.Username))
            return NotFound(new { message = "Account does not exist" });

        var passwordCheck = _passwordHash.CheckPassword(account.Password, account.Username);

        if (passwordCheck.Result is false) return Unauthorized(new { message = "Invalid password" });

        var token = GenerateJwtToken(account.Username);

        return Ok(new { message = "Login successful", token });
    }

    private string GenerateJwtToken(string username)
    {
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}