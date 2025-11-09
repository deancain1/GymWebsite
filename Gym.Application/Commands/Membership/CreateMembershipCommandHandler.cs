using AutoMapper;
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
        private readonly IMapper _mapper;
        public CreateMembershipCommandHandler(IMembershipsRepository membership, IMapper mapper) 
        {
            _membership = membership;
            _mapper = mapper;
        }
        public async Task<MembershipDTO> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
        {
            var member = _mapper.Map<Memberships>(request);

            await _membership.CreateMembershipAsync(member);
            return _mapper.Map<MembershipDTO>(member);
        }
    }
}
