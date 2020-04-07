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
    private RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
      IPasswordHasher<AppUser> passwordHash, IOptions<ApplicationSettings> appSettings, RoleManager<IdentityRole> roleManager)
    {
      this.UserManager = userManager;
      this.SignInManager = signInManager;
      this.passwordHasher = passwordHash;
      this._roleManager = roleManager;
      this.appSettings = appSettings.Value;
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [Route("GetAllRoles")]
    public IEnumerable<RoleModel> GetAllRoles()
    {
      var roles = _roleManager.Roles.ToList();
      var model = new List<RoleModel>();
      roles.ForEach(item => model.Add(
          new RoleModel
          {
            Id = item.Id,
            Name = item.Name
          }
          ));
      return (model);
    }
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [Route("FirstOpen")]
    public IActionResult FirstOpen()
    {
      UserModel model = new UserModel();
      if (UserManager.Users.Count() == 0)
      {
        model.RoleModels = GetAllRoles();
        return Ok(model);
      }

      else
      {
        model.Count = UserManager.Users.Count();
        model.RoleModels = GetAllRoles();
        return Ok(model);
      }
    }


    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [Route("GetLastUser")]
    public IActionResult GetLastUser()
    {

      if (UserManager.Users.Last() == null)
      {
        return Ok(0);
      }
      var LastUser = UserManager.Users.OrderBy(m => m.Creationdate).Last();
      var model = new UserModel();
      model.Id = LastUser.Id;
      model.UserName = LastUser.UserName;
      model.Email = LastUser.Email;
      model.FirstName = LastUser.FirstName;
      model.Password = LastUser.PasswordHash;
      return Ok(model);
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [Route("Paging/{pageNumber}")]
    public async Task<IActionResult> Pagination(int pageNumber)
    {
      if (pageNumber > 0)
      {

        var User = UserManager.Users.OrderBy(m => m.Creationdate).Skip(pageNumber - 1).Take(1).FirstOrDefault();
        if (User == null)
        {
          return Ok(0);
        }
        var model = new UserModel();
        model.Id = User.Id;
        model.UserName = User.UserName;
        model.Email = User.Email;
        model.FirstName = User.FirstName;
        model.LastName = User.LastName;
        model.Password = User.PasswordHash;
        model.ConfirmPassword = User.PasswordHash;
        model.Count = UserManager.Users.Count();
        var UserRoles = await UserManager.GetRolesAsync(User);
        model.Role = UserRoles[0];
        model.RoleModels = GetAllRoles();

        return Ok(model);
      }
      else
        return Ok(1);

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
