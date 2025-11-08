using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Users
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        public GetCurrentUserQueryHandler(IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }
        public async Task<UserDTO> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("User is not authenticated");
            var u = await _userRepository.GetCurrentUserByTokenAsync(userId);
            if (u == null)
                throw new Exception("User not found");
            return new UserDTO
            {
                UserId = u.Id,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                Gender = u.Gender,
                DateOfBirth = u.DateOfBirth,
                Address = u.Address,
                ProfilePicture = u.ProfilePicture,
            };
        }
    }
}
