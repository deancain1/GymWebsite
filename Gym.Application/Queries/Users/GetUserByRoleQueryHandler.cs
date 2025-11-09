using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetUserByRoleQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.GetUsersInRoleAsync(request.RoleName);

            var result = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDTO = _mapper.Map<UserDTO>(user);


                userDTO.Roles = await _userManager.GetRolesAsync(user);

                result.Add(userDTO);
            }

            return result;
        }
    }
}
