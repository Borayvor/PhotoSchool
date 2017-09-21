using Microsoft.AspNetCore.Authorization;

namespace Server.Infrastructure.UserSessionUtils
{
  public class SessionAuthorizeRequirement : IAuthorizationRequirement
  {
    public SessionAuthorizeRequirement()
    {
    }
  }
}
