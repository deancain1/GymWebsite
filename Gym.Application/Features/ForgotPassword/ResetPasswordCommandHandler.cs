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
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordRepostitory _passwordRepository;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IPasswordRepostitory passwordRepository)
        {
            _userManager = userManager;
            _passwordRepository = passwordRepository;
        }
        public async Task<ResetPasswordResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new ResetPasswordResult { IsSuccess = false, Message = "User not found." };

           
            var otp = await _passwordRepository.GetValidOTPAsync(user.Id, request.OtpCode);
            if (otp == null)
                return new ResetPasswordResult { IsSuccess = false, Message = "Invalid or expired OTP." };

           
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
                return new ResetPasswordResult { IsSuccess = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

          
            otp.IsUsed = true;
            await _passwordRepository.SaveAsync();

            return new ResetPasswordResult { IsSuccess = true, Message = "Password reset successfully." };
        }
    }
}
