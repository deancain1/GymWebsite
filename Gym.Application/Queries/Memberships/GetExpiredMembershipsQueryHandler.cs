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
    public class GetExpiredMembershipsQueryHandler : IRequestHandler<GetExpiredMembershipsQuery, List<MembershipsPerMonthDTO>>
    {
        private readonly IMembershipsRepository _membershipRepository;

        public GetExpiredMembershipsQueryHandler(IMembershipsRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<List<MembershipsPerMonthDTO>> Handle(GetExpiredMembershipsQuery request, CancellationToken cancellationToken)
        {
            var data = await _membershipRepository.GetExpiredMembershipsPerMonthAsync();

            var result = data.Select(x => new MembershipsPerMonthDTO
            {
                Month = x.Key,
                Count = x.Value
            })
            .OrderBy(x => DateTime.ParseExact(x.Month, "MMM yyyy", null))
            .ToList();

            return result;
        }
    }
}
