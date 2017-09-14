using System;
using Microsoft.IdentityModel.Tokens;
using Server.Common.Constants;

namespace Server.Auth
{
  public class TokenAuthOption
  {
    public static string Audience { get; } = "http://localhost:55551/";

    public static string Authority { get; } = "http://localhost:55550/";

    public static string Issuer { get; } = "http://localhost:55550/";

    public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());

    public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(
      Key,
      SecurityAlgorithms.RsaSha256Signature);

    public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(AuthConstants.TokenTimeSpanMinutes);

    public static string TokenType { get; } = "Bearer";
  }
}
