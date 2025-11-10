using Gym.Application.Commands.Users;
using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Membership
{
    public class DeleteMembershipsCommandHandler : IRequestHandler<DeleteMembershipsCommand>
    { 
        private readonly IMembershipsRepository _membershipsRepository;
        public DeleteMembershipsCommandHandler(IMembershipsRepository membershipsRepository)
        {
            _membershipsRepository = membershipsRepository;
        }

        public async Task Handle(DeleteMembershipsCommand request, CancellationToken cancellationToken)
        {
            var memberships = await _membershipsRepository.GetMemberByIDAsync(request.MemberID)
                   ?? throw new Exception("Member not found");
             await _membershipsRepository.DeleteMembershipsAsync(memberships);
        }
    }
}
