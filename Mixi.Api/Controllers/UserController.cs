using Microsoft.AspNetCore.Mvc;
using Mixi.Api.Modules.Account;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Users;
using Mixi.Shared.Models.Account;
using Mixi.Shared.Models.UI;
using Org.BouncyCastle.Asn1.X509;

namespace Mixi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHash _passwordHash;
    
    
    public UserController(IUserRepository userRepository, PasswordHash passwordHash)
    {
        _userRepository = userRepository;
        _passwordHash = passwordHash;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Account account)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_userRepository.CheckIfExists((account.Username)))
        {
            Console.WriteLine("dupa");
            return Conflict(new {message = "Account already exists"});
        }
        
        var hashedPassword = _passwordHash.HashPasswords(account.Password, account.Username);

        var newUser = new User
        {
            Username = account.Username,
            Password = hashedPassword,
            UserType = account.UserType
        };
        
        await _userRepository.AddUserAsync(newUser);
        
        return Ok(new {message = "Account created"});;

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Account account)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!_userRepository.CheckIfExists(account.Username))
        {
            return NotFound(new {message = "Account does not exist"});
        }
        
        var passwordCheck = _passwordHash.CheckPassword(account.Password, account.Username);

        if (passwordCheck.Result is false)
        {
            return Unauthorized(new {message = "Invalid password"});   
        }
        else
        {
           return StatusCode( 201, new {message = "Login successful"}); 
        }
     


    }
}