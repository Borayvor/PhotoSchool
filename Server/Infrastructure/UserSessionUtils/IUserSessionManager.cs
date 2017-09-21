using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Data.Models;

namespace Server.Infrastructure.UserSessionUtils
{
  public interface IUserSessionManager
  {
    Task<UserSession> CreateUserSession(User user);

    Task<bool> ReValidateSession(HttpContext httpContext);

    Task<bool> DeleteExpiredSessions(HttpContext httpContext);
  }
}