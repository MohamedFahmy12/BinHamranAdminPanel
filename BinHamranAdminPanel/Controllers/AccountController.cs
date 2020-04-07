using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BinHamranAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BinHamranAdminPanel.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly ApplicationSettings appSettings;

    private UserManager<AppUser> UserManager;
    private SignInManager<AppUser> SignInManager;
    private IPasswordHasher<AppUser> passwordHasher;
    

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
      IPasswordHasher<AppUser> passwordHash, IOptions<ApplicationSettings> appSettings)
    {
      this.UserManager = userManager;
      this.SignInManager = signInManager;
      this.passwordHasher = passwordHash;
      this.appSettings = appSettings.Value;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<object> Regester(UserModel appUser)
    {
      var applicationuser = new AppUser()
      {
        UserName = appUser.UserName,
        Email = appUser.Email,
        FirstName = appUser.FirstName,
        LastName = appUser.LastName
      };
      var result = await UserManager.CreateAsync(applicationuser, appUser.Password);
      if (result.Succeeded)
      {
        await UserManager.AddToRoleAsync(applicationuser, appUser.Role);
        return Ok(result);
      }
      else
      {
        foreach (var item in result.Errors)
        {
          if (item.Code == "DuplicateUserName")
          {
            return Ok(2);
          }
          if (item.Code == "InvalidUserName")
          {
            return Ok(1);
          }

        }
        return Ok(result);
      }

    }
    [HttpPost]
    [Route("Login")]
    //Post:/api/ApplicationUser/Login
    public async Task<IActionResult> Login(LoginModel model)
    {
      var user = await UserManager.FindByNameAsync(model.LoginUserName);
      if (user != null && await UserManager.CheckPasswordAsync(user, model.LoginPassword))
      {
        // Get Role assigned to the user 
        var UserRoles = await UserManager.GetRolesAsync(user);
        IdentityOptions _options = new IdentityOptions();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(new Claim[]
            {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,UserRoles.FirstOrDefault())
            }),
          Expires = DateTime.UtcNow.AddDays(1),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Secret)),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return Ok(new { token });
      }
      else
        return BadRequest(new { message = "Username or Password is incorrect." });
    }
    [HttpPut]
    [Authorize(Roles = "SuperAdmin")]
    [Route("~/api/ApplicationUser/PutUser/{id}")]
    public async Task<object> PutUser(string id, [FromBody] UserModel model)
    {
      var user = await UserManager.FindByIdAsync(id);
      var UserRoles = await UserManager.GetRolesAsync(user);
      string OldRole = UserRoles[0];
      if (user != null)
      {
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        try
        {
          var result = await UserManager.UpdateAsync(user);
          if (result.Succeeded)
          {
            await UserManager.RemoveFromRoleAsync(user, OldRole);
            await UserManager.AddToRoleAsync(user, model.Role);
            return Ok(result);
          }
          else
          {
            foreach (var item in result.Errors)
            {
              if (item.Code == "DuplicateUserName")
              {
                return Ok(2);
              }
              if (item.Code == "InvalidUserName")
              {
                return Ok(1);
              }
            }
            return Ok(result);
          }
        }
        catch (DbUpdateException ex)
        {
          var sqlException = ex.GetBaseException() as SqlException;

          if (sqlException != null)
          {
            var number = sqlException.Number;

            if (number == 547)
            {
              return Ok(5);

            }
            else
              return Ok(6);
          }
          return Ok(6);

        }
      }
      else
      {
        return Ok(0);
      }



    }
    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    [Route("~/api/ApplicationUser/DeleteUser/{id}")]

    public async Task<object> DeleteUser(string id)
    {
      var user = await UserManager.FindByIdAsync(id);
      var UserRoles = await UserManager.GetRolesAsync(user);
      string UserRole = UserRoles[0];
      if (user != null)
      {
        try
        {
          await UserManager.RemoveFromRoleAsync(user, UserRole);
          var result = await UserManager.DeleteAsync(user);
          return Ok(result);

        }
        catch (DbUpdateException ex)
        {
          var sqlException = ex.GetBaseException() as SqlException;

          if (sqlException != null)
          {
            var number = sqlException.Number;

            if (number == 547)
            {
              return Ok(5);

            }
            else
              return Ok(6);
          }
          return Ok(6);

        }

      }
      else
      {
        return Ok(0);
      }


    }
  }
}
