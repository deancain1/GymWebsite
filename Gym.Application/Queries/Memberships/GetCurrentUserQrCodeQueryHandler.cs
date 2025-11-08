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
    public class GetCurrentUserQrCodeQueryHandler : IRequestHandler<GetCurrentUserQrCodeQuery, MembershipDTO>
    {
        private readonly IMembershipsRepository _membershipRepository;
        private readonly ICurrentUserService _currentUser;
        public GetCurrentUserQrCodeQueryHandler(IMembershipsRepository membershipRepository, ICurrentUserService currentUser)
        {
            _membershipRepository = membershipRepository;
            _currentUser = currentUser;
        }
        public async Task<MembershipDTO> Handle(GetCurrentUserQrCodeQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated");

            var membership = await _membershipRepository.GetQrCodeByUserIdAsync(userId);
            if (membership == null)
                throw new Exception("Membership not found for the current user.");

            return new MembershipDTO
            {
                UserId = membership.UserId,
                QRCode = membership.QRCode
            };
        }
    }
}
