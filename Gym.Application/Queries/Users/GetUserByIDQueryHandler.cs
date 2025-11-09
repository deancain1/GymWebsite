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
    public class GetUserByIDQueryHandler : IRequestHandler<GetUserByIDQuery, UserDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUserByIDQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO?> Handle(GetUserByIDQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null) return null;
            return _mapper.Map<UserDTO>(user);
        }
    }

}