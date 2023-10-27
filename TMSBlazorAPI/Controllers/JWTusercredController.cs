using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMSBlazorAPI.Data;
using TMSBlazorAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using TMSBlazorAPI.Handler;

namespace TMSBlazorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTusercredController : ControllerBase
    {
        private readonly TMSDbContext tMSDbContext;
        private readonly JwtSettings jwtSettings;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        public JWTusercredController(TMSDbContext tMSDbContext, IOptions<JwtSettings> options, IRefreshTokenGenerator refresh)
        {
            this.tMSDbContext = tMSDbContext;
            this.jwtSettings = options.Value;
            this.refreshTokenGenerator = refresh;
        }

        [NonAction]
        public async Task<TokenResponse> TokenAuthenticate(string username, Claim[] claims)
        {
            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddSeconds(20),
                signingCredentials:new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.securitykey)),SecurityAlgorithms.HmacSha256)
                );
            var jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenResponse()
            {
                jwttoken = jwttoken,
                refreshtoken = await refreshTokenGenerator.GenerateToken(username)
            };
        }

        [HttpPost("Authorization")]
        public async Task<IActionResult> Authenticate([FromBody] JWTAuthenticationCred jWTAuthenticationCred )
        {
            var user = await this.tMSDbContext.Users.FirstOrDefaultAsync(item => item.Email == jWTAuthenticationCred.UserName && item.Password == jWTAuthenticationCred.Password);
            if (user == null)           
            return Unauthorized();

            //generate token
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role) 
                    }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokendesc);
            string finaltoken = tokenhandler.WriteToken(token);

            var response = new TokenResponse()
            {
                jwttoken = finaltoken,
                refreshtoken =await refreshTokenGenerator.GenerateToken(jWTAuthenticationCred.UserName)
            };

            return Ok(response);
        }


        [HttpPost("RefToken")]
        public async Task<IActionResult> RefToken([FromBody] TokenResponse tokenResponse)
        {

            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
            SecurityToken securityToken;
            var principal = tokenhandler.ValidateToken(tokenResponse.jwttoken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenkey),
                ValidateIssuer = false,
                ValidateAudience = false,

            }, out securityToken);

            var token = securityToken as JwtSecurityToken;
            if (token != null && !token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }

            var username = principal.Identity?.Name;

            var user = await this.tMSDbContext.Refreshtokens.FirstOrDefaultAsync(item => item.Email == username && item.Refreshtoken1 == tokenResponse.refreshtoken);
            if (user == null)
                return Unauthorized(tokenResponse);

            var response = TokenAuthenticate(username, principal.Claims.ToArray()).Result;
            return Ok(response);
        }
    }
}
