using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Auth;
using Server.Data.Common;
using Server.Data.Models;
using Server.EnumTypes;
using Server.ViewModels;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  [AutoValidateAntiforgeryToken]
  public class AuthController : Controller
  {
    private readonly IMongoDbRepository<User> users;

    public AuthController(IMongoDbRepository<User> users)
    {
      this.users = users;
    }

    [HttpPost("Register")]
    public async Task<string> Register([FromBody]UserViewModel user)
    {
      if (!this.ModelState.IsValid)
      {
        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Failed
        });
      }

      var existUser = this.users.GetAll().Result.FirstOrDefault(u => u.Username == user.Username);

      if (existUser == null)
      {
        var newUser = new User()
        {
          Username = user.Username,
          Password = user.Password
        };

        var createdUser = await this.users.Create(newUser);

        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Success,
          Data = new
          {
            UserId = createdUser.Id
          }
        });
      }
      else
      {
        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Failed,
          Msg = "This Username exist !"
        });
      }
    }

    [HttpPost("Login")]
    public async Task<string> Login([FromBody]UserViewModel user)
    {
      if (!this.ModelState.IsValid)
      {
        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Failed
        });
      }

      var allUsers = await this.users.GetAll();
      var existUser = allUsers.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

      if (existUser != null)
      {
        var requestAt = DateTime.UtcNow;
        var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
        var token = this.GenerateToken(existUser, expiresIn);

        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Success,
          Data = new
          {
            requertAt = requestAt,
            expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
            tokeyType = TokenAuthOption.TokenType,
            accessToken = token
          }
        });
      }
      else
      {
        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Failed,
          Msg = "Username or password is invalid"
        });
      }
    }

    [HttpGet("Logout")]
    [Authorize("Bearer")]
    public async Task<string> Logout()
    {
      // TODO: implement Logout
      Console.WriteLine("Logout !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");  
   

      return JsonConvert.SerializeObject(new RequestResultViewModel
      {
        State = RequestState.Success,
        Msg = signOutResult.ToString()
      });
    }

    [HttpGet("GetUserInfo")]
    [Authorize("Bearer")]
    public string GetUserInfo()
    {
      var claimsIdentity = this.User.Identity as ClaimsIdentity;        

      return JsonConvert.SerializeObject(new RequestResultViewModel
      {
        State = RequestState.Success,
        Data = new
        {
          UserName = claimsIdentity.Name
        }
      });
    }

    private string GenerateToken(User user, DateTime expires)
    {
      var handler = new JwtSecurityTokenHandler();

      ClaimsIdentity identity = new ClaimsIdentity(
          new GenericIdentity(user.Username, "TokenAuth"),
          new[] { new Claim("Id", user.Id.ToString()) });

      this.User.AddIdentity(identity);

      var securityToken = handler.CreateToken(new SecurityTokenDescriptor
      {
        Issuer = TokenAuthOption.Issuer,
        Audience = TokenAuthOption.Audience,
        SigningCredentials = TokenAuthOption.SigningCredentials,
        Subject = identity,
        Expires = expires
      });

      return handler.WriteToken(securityToken);
    }
  }
}