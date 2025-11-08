using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Memberships
{
    public class GetTotalMembershipsQueryHandler : IRequestHandler<GetTotalMembershipsQuery, int>
    {
        private readonly IMembershipsRepository _membershipRepository;
        public GetTotalMembershipsQueryHandler(IMembershipsRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }
        public async Task<int> Handle(GetTotalMembershipsQuery request, CancellationToken cancellationToken)
        {
            return await _membershipRepository.GetTotalMembershipsAsync();
        }
    }
}
