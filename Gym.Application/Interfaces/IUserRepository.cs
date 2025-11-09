using Gym.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(ApplicationUser user);
        Task<int> GetTotalUserAsync();
        Task<int> GetTotalAdminsAsync();
        Task<ApplicationUser?> GetCurrentUserByTokenAsync(string userId);
    }
}
