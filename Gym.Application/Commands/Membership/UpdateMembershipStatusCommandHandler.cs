using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Membership
{
    public class UpdateMembershipStatusCommandHandler : IRequestHandler<UpdateMembershipStatusCommand>
    {
        private readonly IMembershipsRepository _membershipsRepository;
        private readonly IQRCodeService _qRCodeService;

        public UpdateMembershipStatusCommandHandler(IMembershipsRepository membershipsRepository, IQRCodeService qRCodeService)
        {
            _membershipsRepository = membershipsRepository;
            _qRCodeService = qRCodeService;
        }

        public async Task Handle(UpdateMembershipStatusCommand request, CancellationToken cancellationToken)
        {
            var member = await _membershipsRepository.GetMemberByIDAsync(request.MemberID);
            if (member == null) throw new Exception("Membership not found");

            member.Status = request.Status;

            if (request.Status == "Accepted")
            {
                member.StartDate = DateTime.UtcNow;
                member.ExpirationDate = member.StartDate.Value.AddMonths(member.DurationMonths);


                var qrContent = $@"
                Member ID: {member.MemberID}";

                member.QRCode = _qRCodeService.GenerateQRCode(qrContent);
            }

            await _membershipsRepository.UpdateMembershipStatusAsync(member);
        }
    }
}
