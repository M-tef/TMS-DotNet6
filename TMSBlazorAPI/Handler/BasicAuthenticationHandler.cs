using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
//using System.Text.Encodings.Web;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text;
using TMSBlazorAPI.Handler;
using TMSBlazorAPI.Data;
using System.Security.Claims;

namespace TMSBlazorAPI.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private TMSDbContext _dbContext;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, TMSDbContext dbContext) 
            : base(options, logger, encoder, clock) 
        {
            _dbContext = dbContext;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No Header Found");
            }

            var _headervalue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(_headervalue.Parameter);
            string credentials = Encoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(":");
                string username = array[0];
                string password = array[1];

                var user = this._dbContext.Users.FirstOrDefault(item => item.Email == username || item.Username == username && item.Password == password);
                if(user == null)
                {
                    return AuthenticateResult.Fail("Unauthorized");
                }

                var claim = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claim, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal,Scheme.Name);


                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
        }
    }
}
