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

namespace TMSBlazorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTusercredController : ControllerBase
    {
        private readonly TMSDbContext tMSDbContext;
        private readonly JwtSettings jwtSettings;

        public JWTusercredController(TMSDbContext tMSDbContext, IOptions<JwtSettings> options)
        {
            this.tMSDbContext = tMSDbContext;
            this.jwtSettings = options.Value;
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
                    new Claim[] { new Claim(ClaimTypes.Name, user.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokendesc);
            string finaltoken = tokenhandler.WriteToken(token);
             
            return Ok(finaltoken);
        }
    }
}
