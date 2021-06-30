using OrganizerAPI.Shared.ModelsDTO;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IUserService : IService<UserDto>
    {
        Task<UserAuthResponseDto> Registration(UserRequestDto model, string ipAddress);
        Task<UserAuthResponseDto> Authenticate(UserAuthRequestDto model, string ipAddress);
        Task<UserAuthResponseDto> UpdateRefreshToken(string token, string ipAddress);
        Task RevokeToken(string token, string ipAddress);
        Task<UserDto> GetCurrentUser(string token);
        Task<int> GetCurrentUserId(string token);
    }
}
