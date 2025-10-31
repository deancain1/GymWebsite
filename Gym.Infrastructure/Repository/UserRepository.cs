using Gym.Domain.Interfaces;
using Gym.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<int> GetTotalAdminsAsync()
        {
            var admins = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (admins == null)
                return 0;
            var count = await _context.UserRoles.CountAsync(ur => ur.RoleId == admins.Id);
            return count;
        }

        public async Task<int> GetTotalUserAsync()
        {
          var user = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (user == null)
                return 0;
            var count = await _context.UserRoles.CountAsync(ur => ur.RoleId == user.Id);
            return count;
        }

    }
}
