namespace TMSBlazorAPI.Handler
{
    public interface IRefreshTokenGenerator
    {
        Task<string> GenerateToken(string username);
    }
}
