using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Memberships
{
    public class GetCountMembersipTypeQueryHandler : IRequestHandler<GetCountMembersipTypeQuery, List<MembershipPlanDTO>>
    {
        private readonly IMembershipsRepository _membershipRepository;
        public GetCountMembersipTypeQueryHandler(IMembershipsRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }
        public async Task<List<MembershipPlanDTO>> Handle(GetCountMembersipTypeQuery request, CancellationToken cancellationToken)
        {
          

            var data = await _membershipRepository.GetMembershipPlanCountsAsync();

            var result = data.Select(x => new MembershipPlanDTO
            {
                MembershipPlan = x.Key,
                Count = x.Value
            })
            .ToList();

            return result;

        }
    }
}
