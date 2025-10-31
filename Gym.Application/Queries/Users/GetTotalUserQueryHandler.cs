using Gym.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Users
{
    public class GetTotalUserQueryHandler : IRequestHandler<GetTotalUserQuery, int>
    {
        private readonly IUserRepository _userRepository;
        public GetTotalUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(GetTotalUserQuery request, CancellationToken cancellationToken)
        {
            var totalUsers = await _userRepository.GetTotalUserAsync();
            return totalUsers;
        }
    }
}
