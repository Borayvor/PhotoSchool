using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Server.Auth;
using Server.Common.Constants;
using Server.Data.Common;
using Server.Data.Models;

namespace Server.Infrastructure.UserSessionUtils
{
  public class UserSessionManager : IUserSessionManager
  {
    private const string AuthenticationTypeString = "TokenAuth";
    private const string ClaimTypeIdString = "Id";

    private readonly IMongoDbRepository<UserSession> userSessions;

    public UserSessionManager(IMongoDbRepository<UserSession> userSessions)
    {
      this.userSessions = userSessions;
    }

    public async Task<UserSession> CreateUserSession(User user)
    {
      var requestAt = DateTime.UtcNow;
      var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
      var authToken = this.GenerateToken(user, expiresIn);

      var newUserSession = new UserSession()
      {
        OwnerUserId = user.Id,
        AuthToken = authToken,
        ExpirationDateTime = DateTime.UtcNow + TimeSpan.FromMinutes(AuthConstants.TokenTimeSpanMinutes)
      };

      var userSession = await this.userSessions.Create(newUserSession);

      return userSession;
    }

    public async Task<bool> ReValidateSession(HttpContext httpContext)
    {
      string authToken = this.GetCurrentBearerAuthorizationToken(httpContext);
      var currentUserId = this.GetCurrentUserId(httpContext);
      var currentUserSessions = await this.userSessions.GetAll();
      var currentUserSession = currentUserSessions.FirstOrDefault(session =>
          session.AuthToken == authToken && session.OwnerUserId == currentUserId);

      if (currentUserSession == null)
      {
        // User does not have a session with this token --> invalid session
        return false;
      }

      if (currentUserSession.ExpirationDateTime < DateTime.UtcNow)
      {
        // User's session is expired --> invalid session
        return false;
      }

      // Extend the lifetime of the current user's session: current moment + fixed timeout
      currentUserSession.ExpirationDateTime = DateTime.UtcNow +
      TimeSpan.FromMinutes(AuthConstants.TokenTimeSpanMinutes);

      var isReValidated = await this.userSessions.Update(currentUserSession);

      return true;
    }

    public async Task<bool> DeleteExpiredSessions(HttpContext httpContext)
    {
      var isDeleted = true;
      var currentUserSessions = await this.userSessions.GetAll();
      var currentUserId = httpContext.User.FindFirst(ClaimTypeIdString).Value;
      var currentUserSession = currentUserSessions
        .FirstOrDefault(session =>
        session.OwnerUserId == currentUserId &&
        session.ExpirationDateTime < DateTime.UtcNow);

      if (currentUserSession != null)
      {
        isDeleted = await this.userSessions.Delete(currentUserSession.Id);
      }

      return isDeleted;
    }

    private string GetCurrentBearerAuthorizationToken(HttpContext httpContext)
    {
      string authToken = null;

      var headerAuth = (string)httpContext.Request.Headers["Authorization"];

      if (!string.IsNullOrWhiteSpace(headerAuth))
      {
        var authTokenData = headerAuth.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (authTokenData[0] == "Bearer")
        {
          authToken = authTokenData[1];
        }
      }

      return authToken;
    }

    private string GetCurrentUserId(HttpContext httpContext)
    {
      var claimId = httpContext.User.FindFirst(ClaimTypeIdString);

      if (claimId == null)
      {
        return null;
      }

      string userId = claimId.Value;

      return userId;
    }

    private string GenerateToken(User user, DateTime expires)
    {
      var handler = new JwtSecurityTokenHandler();

      ClaimsIdentity identity = new ClaimsIdentity(
          new GenericIdentity(user.Username, AuthenticationTypeString),
          new[] { new Claim(ClaimTypeIdString, user.Id.ToString()) });

      var securityToken = handler.CreateToken(new SecurityTokenDescriptor
      {
        Issuer = TokenAuthOption.Issuer,
        Audience = TokenAuthOption.Audience,
        SigningCredentials = TokenAuthOption.SigningCredentials,
        Subject = identity,
        Expires = expires
      });

      var token = handler.WriteToken(securityToken);

      return token;
    }
  }
}