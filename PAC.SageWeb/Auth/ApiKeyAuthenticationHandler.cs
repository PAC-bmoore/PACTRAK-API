using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace PAC.SageWeb.Auth
{
    /// <summary>
    /// A caller presents one of the issued keys in the X-Api-Key header.
    /// Keys live in configuration (the ApiKeys section: one entry per
    /// caller, name to key), never in code; the entry name becomes the
    /// caller's identity in the logs. Sits beside the Entra JWT scheme so
    /// either proves a caller.
    /// </summary>
    public sealed class ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IConfiguration configuration)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        public const string SchemeName = "ApiKey";
        public const string HeaderName = "X-Api-Key";

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderName, out var presented) || string.IsNullOrWhiteSpace(presented))
                return Task.FromResult(AuthenticateResult.NoResult());

            var given = Encoding.UTF8.GetBytes(presented.ToString());
            foreach (var entry in configuration.GetSection("ApiKeys").GetChildren())
            {
                if (string.IsNullOrEmpty(entry.Value)) continue;
                if (CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(entry.Value), given))
                {
                    var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, entry.Key)], SchemeName);
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName)));
                }
            }
            return Task.FromResult(AuthenticateResult.Fail("Unknown API key."));
        }
    }
}
