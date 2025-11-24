using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Features.ForgotPassword
{
    public class VerifyOTPCOmmandHandler : IRequestHandler<VerifyOTPCOmmand, VerifyOTPResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordRepostitory _passwordRepository;

        public VerifyOTPCOmmandHandler(
            UserManager<ApplicationUser> userManager,
            IPasswordRepostitory passwordRepository)
        {
            _userManager = userManager;
            _passwordRepository = passwordRepository;
        }

        public async Task<VerifyOTPResult> Handle(VerifyOTPCOmmand request, CancellationToken ct)
        {
         
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new VerifyOTPResult { IsValid = false, Message = "User not found." };

            
            var otp = await _passwordRepository.GetValidOTPAsync(user.Id, request.OtpCode);
            if (otp == null)
                return new VerifyOTPResult { IsValid = false, Message = "Invalid or expired OTP." };


            return new VerifyOTPResult
            {
                IsValid = true,
                Message = "OTP verified successfully."
            };
        }
    }
}
