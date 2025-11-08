using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Memberships
{
    public class GetMembershipsPerMonthQueryHandler : IRequestHandler<GetMembershipsPerMonthQuery, List<MembershipsPerMonthDTO>>
    {
        private readonly IMembershipsRepository _membershipRepository;

        public GetMembershipsPerMonthQueryHandler(IMembershipsRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<List<MembershipsPerMonthDTO>> Handle(GetMembershipsPerMonthQuery request, CancellationToken cancellationToken)
        {
            var data = await _membershipRepository.GetMembershipsPerMonthAsync();

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
