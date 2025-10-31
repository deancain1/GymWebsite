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
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetCurrentUserQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDTO> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToArray()
            };
        }
    }
}
