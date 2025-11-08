using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Membership
{
    public class CreateMembershipCommandHandler : IRequestHandler<CreateMembershipCommand, MembershipDTO>
    {
        private readonly IMembershipsRepository _membership;
        public CreateMembershipCommandHandler(IMembershipsRepository membership)
        {
            _membership = membership;
        }
        public async Task<MembershipDTO> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
        {
            var member = new Memberships
            {
                UserId = request.UserId,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Plan = request.Plan,
                AppliedDate = DateTime.Now,
                DurationMonths = request.DurationMonths,
                Status = "New"
            };

            await _membership.CreateMembershipAsync(member);
            return new MembershipDTO
            {

                FullName = member.FullName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                Address = member.Address,
                Plan = member.Plan,
                AppliedDate = member.AppliedDate,
            };
        }
    }
}
