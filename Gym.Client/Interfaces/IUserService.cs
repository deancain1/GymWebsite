using Gym.Client.DTOs;

namespace Gym.Client.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateAccountAsync(UserDTO userDTO);
        Task<HttpResponseMessage> LoginAsync(LoginRequestDTO loginModel);

        Task<List<UserDTO>> GetAccountsByRoleAsync(string roleName);
        Task<UserDTO?> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> DeleteUserAsync(string UserId);
        Task<int> GetTotalUserAsync();
        Task <int> GetTotalAdminsAsync();
        Task<UserDTO?> GetCurrentUserByTokenAsync();
    }
}
