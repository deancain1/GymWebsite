using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Users
{
    public class GetTotalAdminQueryHandler : IRequestHandler<GetTotalAdminQuery, int>
    {
        private readonly IUserRepository _userRepository;
        public GetTotalAdminQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
 

        public async Task<int> Handle(GetTotalAdminQuery request, CancellationToken cancellationToken)
        {
            var totalAdmins = await _userRepository.GetTotalAdminsAsync();
            return totalAdmins;
        }
    }
}
