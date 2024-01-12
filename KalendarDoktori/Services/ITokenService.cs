using KalendarDoktori.Models;

namespace KalendarDoktori.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
