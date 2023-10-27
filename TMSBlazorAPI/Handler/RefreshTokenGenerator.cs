using System.Security.Cryptography;
using TMSBlazorAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace TMSBlazorAPI.Handler
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly TMSDbContext _DbContext;

        public RefreshTokenGenerator(TMSDbContext tMSDbContext)
        {
            this._DbContext = tMSDbContext;
        }

        public async Task<string> GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string refreshtoken = Convert.ToBase64String(randomnumber);

                var token = await this._DbContext.Refreshtokens.FirstOrDefaultAsync(item => item.Email == username);

                if (token != null)
                {
                    token.Refreshtoken1 = refreshtoken;
                }
                else
                {
                    this._DbContext.Add(new Refreshtoken()
                    {
                        Email = username,
                        TokenId = new Random().Next().ToString(),
                        Refreshtoken1 = refreshtoken,
                        IsActive = true
                    });
                }
                await this._DbContext.SaveChangesAsync();

                return refreshtoken;
            }
        }
    }
}
