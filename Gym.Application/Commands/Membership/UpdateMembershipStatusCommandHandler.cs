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
        private readonly IEmailService _emailService;

        public UpdateMembershipStatusCommandHandler(IMembershipsRepository membershipsRepository, IQRCodeService qRCodeService, IEmailService emailService)
        {
            _membershipsRepository = membershipsRepository;
            _qRCodeService = qRCodeService;
            _emailService = emailService;
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

                var qrContent = $@"Member ID: {member.MemberID}";
                member.QRCode = _qRCodeService.GenerateQRCode(qrContent);

                var emailBody = $@"
                    <h3>Dear {member.FullName},</h3>
                    <p>Your membership has been <strong>accepted</strong>.</p>
                    <p>You can claim your membership card at the gym. 
                    If not claimed, your membership will be deleted.</p>";

                await _emailService.SendEmailAsync(member.Email, "Membership Accepted", emailBody);
            }

           
            else if (request.Status == "Rejected")
            {
                var emailBody = $@"
                    <h3>Dear {member.FullName},</h3>
                    <p>We regret to inform you that your membership request has been <strong>rejected</strong>.</p>";

                await _emailService.SendEmailAsync(member.Email, "Membership Rejected", emailBody);
            }

            await _membershipsRepository.UpdateMembershipStatusAsync(member);
        }
    }
}
