using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Auth;
using Server.Data.Common;
using Server.Data.Models;
using Server.EnumTypes;
using Server.Infrastructure.UserSessionUtils;
using Server.ViewModels;

namespace Server.Controllers
{
  [Route("api/[controller]/[action]")]
  [AutoValidateAntiforgeryToken]
  public class AuthController : Controller
  {
    private readonly IMongoDbRepository<User> users;
    private readonly IUserSessionManager userSessionManager;

    public AuthController(IMongoDbRepository<User> users, IUserSessionManager userSessionManager)
    {
      this.users = users;
      this.userSessionManager = userSessionManager;
    }

    [HttpPost]
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

    [HttpPost]
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
        var userSession = await this.userSessionManager.CreateUserSession(existUser);
        var isUserSessionDeleted = await this.userSessionManager.DeleteExpiredSessions(this.HttpContext);

        return JsonConvert.SerializeObject(new RequestResultViewModel
        {
          State = RequestState.Success,
          Data = new
          {
            requertAt = userSession.CreatedOn,
            expiresIn = userSession.ExpirationDateTime,
            tokeyType = TokenAuthOption.TokenType,
            accessToken = userSession.AuthToken
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

    [HttpGet]
    [Authorize("Bearer")]
    public string Logout()
    {
      // TODO: implement Logout
      Console.WriteLine("Logout !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

      return JsonConvert.SerializeObject(new RequestResultViewModel
      {
        State = RequestState.Success
      });
    }

    [HttpGet]
    [Authorize("Bearer")]
    [Authorize("SessionAuth")]
    public string GetUserInfo()
    {
      var headerAuth = (string)this.Request.Headers["Authorization"];
      var token = string.Empty;

      if (!string.IsNullOrWhiteSpace(headerAuth))
      {
        token = headerAuth.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
      }

      return JsonConvert.SerializeObject(new RequestResultViewModel
      {
        State = RequestState.Success,
        Data = new
        {
          Username = this.User.Identity.Name,
          Token = token
        }
      });
    }
  }
}