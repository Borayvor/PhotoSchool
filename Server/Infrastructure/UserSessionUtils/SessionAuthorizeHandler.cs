using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Server.Infrastructure.UserSessionUtils
{
  public class SessionAuthorizeHandler : AuthorizationHandler<SessionAuthorizeRequirement>
  {
    private readonly IUserSessionManager userSessionManager;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly HttpContext httpContext;

    public SessionAuthorizeHandler(
      IUserSessionManager userSessionManager,
      IHttpContextAccessor httpContextAccessor)
    {
      this.userSessionManager = userSessionManager;
      this.httpContextAccessor = httpContextAccessor;
      this.httpContext = this.httpContextAccessor.HttpContext;
    }

    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      SessionAuthorizeRequirement requirement)
    {
      if (this.userSessionManager.ReValidateSession(this.httpContext).Result)
      {
        context.Succeed(requirement);
      }
      else
      {
        context.Fail();
      }

      return Task.CompletedTask;
    }
  }
}