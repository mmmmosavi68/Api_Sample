using System.Threading.Tasks;
using Entities;

namespace Services.Services
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}