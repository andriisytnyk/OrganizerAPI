using OrganizerAPI.Shared.ModelsDTO;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IUserService : IService<UserDto>
    {
        Task<UserAuthResponseDto> Registration(UserRequestDto model, string ipAddress);
        UserAuthResponseDto Authenticate(UserAuthRequestDto model, string ipAddress);
        UserAuthResponseDto UpdateRefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        UserDto GetCurrentUser(string token);
        int GetCurrentUserId(string token);
    }
}
