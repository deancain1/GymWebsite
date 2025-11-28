using Gym.Client.DTOs;

namespace Gym.Client.Interfaces
{
    public interface IAuthService
    {
        Task<bool> CreateAccountAsync(UserDTO userDTO);
        Task<HttpResponseMessage> LoginAsync(LoginRequestDTO loginModel);
    }
}
