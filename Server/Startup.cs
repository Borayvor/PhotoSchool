using System;
using System.IO;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Server.Auth;
using Server.Data;
using Server.Data.Common;
using Server.Infrastructure.UserSessionUtils;

namespace Server
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();

      MongoDbClassMapsConfig.RegisterAllMaps();

      this.Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      // Angular's default header name for sending the XSRF token.
      services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.Configuration = new OpenIdConnectConfiguration();

        // ONLY FOR DEVELOPMENT !!!
        options.RequireHttpsMetadata = false;

        options.Audience = TokenAuthOption.Audience;
        options.Authority = TokenAuthOption.Authority;

        options.TokenValidationParameters.IssuerSigningKey = TokenAuthOption.Key;
        options.TokenValidationParameters.ValidAudience = TokenAuthOption.Audience;
        options.TokenValidationParameters.ValidIssuer = TokenAuthOption.Issuer;
        options.TokenValidationParameters.ValidateIssuer = true;

        // When receiving a token, check that we've signed it.
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;

        // When receiving a token, check that it is still valid.
        options.TokenValidationParameters.ValidateLifetime = true;

        // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
        // when validating the lifetime. As we're creating the tokens locally and validating them on the same
        // machines which should have synchronised time, this can be set to zero. Where external tokens are
        // used, some leeway here could be useful.
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
      });

      // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
      services.AddAuthorization(auth =>
      {
        auth.AddPolicy(
          "Bearer",
          new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
          .RequireAuthenticatedUser()
          .Build());

        auth.AddPolicy(
         "SessionAuth",
         policy => policy.Requirements.Add(new SessionAuthorizeRequirement()));
      });

      services.AddSingleton<IAuthorizationHandler, SessionAuthorizeHandler>();

      // Add framework services.
      services.AddMvc();
      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.Configure<MongoDbSettings>(options =>
      {
        options.ConnectionString = this.Configuration.GetSection("MongoConnection:ConnectionString").Value;
        options.DatabaseName = this.Configuration.GetSection("MongoConnection:DatabaseName").Value;
        options.IsSSL = Convert.ToBoolean(this.Configuration.GetSection("MongoConnection:IsSSL").Value);
      });

      services.AddScoped<IMongoDbContext, MongoDbContext>();
      services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
      services.AddSingleton<IUserSessionManager, UserSessionManager>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      ILoggerFactory loggerFactory,
      IAntiforgery antiforgery)
    {
      loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.Use(async (context, next) =>
      {
        await next();

        string path = context.Request.Path.Value;

        if (
          context.Response.StatusCode == 404 &&
          !Path.HasExtension(path) &&
          !path.ToLower().Contains("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      app.Use(async (context, next) =>
      {
        string path = context.Request.Path.Value;

        if (
        !Path.HasExtension(path) &&
        path.ToLower().Contains("/api/"))
        {
          // XSRF-TOKEN used by angular in the $http if provided
          var tokens = antiforgery.GetAndStoreTokens(context);

          context.Response.Cookies.Append(
            "XSRF-TOKEN",
            tokens.RequestToken,
            new CookieOptions { HttpOnly = false, Secure = false });
        }

        await next();
      });

      app.UseAuthentication();
      app.UseMvcWithDefaultRoute();
      app.UseDefaultFiles();
      app.UseStaticFiles();
    }
  }
}