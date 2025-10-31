using Gym.Application.DTOs;
using Gym.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Memberships
{
    public class GetAllMembershipsQueryHandler : IRequestHandler<GetAllMembershipsQuery, List<MembershipDTO>>
    {
        private readonly IMembershipRepository _membershipRepository;
        public GetAllMembershipsQueryHandler (IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }
        public async  Task<List<MembershipDTO>> Handle(GetAllMembershipsQuery request, CancellationToken cancellationToken)
        {
            var members = await _membershipRepository.GetAllMembershipsAsync();
            return members.Select(members => new MembershipDTO
            {
                MemberID = members.MemberID,
                FullName = members.FullName,
                Email = members.Email,
                PhoneNumber = members.PhoneNumber,
                Address = members.Address,
                AppliedDate = members.AppliedDate,
                Status = members.Status,
                QRCode = members.QRCode,
            }).ToList();
        }
    }
}
