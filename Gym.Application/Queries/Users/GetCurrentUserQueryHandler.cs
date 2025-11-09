using AutoMapper;
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
        private readonly IMapper _mapper;
        public GetCurrentUserQueryHandler(IUserRepository userRepository, ICurrentUserService currentUser, IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("User is not authenticated");
            var user = await _userRepository.GetCurrentUserByTokenAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            return _mapper.Map<UserDTO>(user);
        }
    }
}
