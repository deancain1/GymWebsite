using Gym.Application.DTOs;
using Gym.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Users
{
    public class GetUserByRoleQueryHandler : IRequestHandler<GetUserByRoleQuery, List<UserDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByRoleQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDTO>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.GetUsersInRoleAsync(request.RoleName);

            var result = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserDTO
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Address = user.Address,
                    Role = user.Role,
                    Roles = roles
                });
            }

            return result;
        }
    }
}
