using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace country.back {
    public class Authentication: AuthenticationHandler<AuthenticationSchemeOptions> {
        public Authentication(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            Endpoint endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null) {
                return AuthenticateResult.NoResult();
            }
            if (!Request.Headers.ContainsKey("Authorization")) {
                return AuthenticateResult.Fail("N?o autorizado.");
            }
            AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            byte[] credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            string username = credentials[0];
            string password = credentials[1];
            User user = new User();
            user = await user.Authenticate(username, password);
            if (user == null) {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }
            Claim[] claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}