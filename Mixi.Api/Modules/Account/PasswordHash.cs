﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Mixi.Api.Modules.Database.Repositories.UserRepositories;
using Mixi.Api.Modules.Enums;
using Mixi.Api.Modules.Users;

namespace Mixi.Api.Modules.Account;

public  class PasswordHash
{
    private PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
    private string? _hashedPassword = String.Empty ;
    public bool LoggedIn { get; set; } = false;
    
    
    private User _user = new();

    public PasswordHash(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

   

    [Inject] private IUserRepository UserRepository { get; set; }
   

    public  string? HashPasswords(string? password, string username)
    {
       _hashedPassword = _passwordHasher.HashPassword(username, password ?? throw new ArgumentNullException(nameof(password)));

       return _hashedPassword;
    }

    public async Task<bool> CheckPassword(string? password, string username)
    {
        
        var user = await UserRepository.GetUserAsync(username);
        
        if (user is null)
        {
            return false;
        }
         _hashedPassword = user.Password;
        
  
         var passwordCheck = _passwordHasher.VerifyHashedPassword(username, _hashedPassword ?? throw new InvalidOperationException(), password ?? throw new ArgumentNullException(nameof(password)));
         if (passwordCheck is PasswordVerificationResult.Success)
         {
             LoggedIn = true;
         }
         return passwordCheck switch
         {
             PasswordVerificationResult.Failed => false,
             PasswordVerificationResult.Success => true,
             _ => false
         };
    }
}