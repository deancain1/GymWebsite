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
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string>
    {
          private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordRepostitory _passwordRepository;
        private readonly IEmailService _emailService;
        public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IPasswordRepostitory passwordRepository, IEmailService emailService)
        {
            _userManager = userManager;
            _passwordRepository = passwordRepository;
            _emailService = emailService;
        }

        public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return "User not found.";

          
            var otp = new Random().Next(100000, 999999).ToString();

            var token = new PasswordResetOTP
            {
                UserId = user.Id,
                OTP = otp,
                Email = user.Email,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

           
            await _passwordRepository.AddAsync(token);
            await _passwordRepository.SaveAsync();  

           
            await _emailService.SendOtpAsync(user.Email!, otp);

            return "OTP has been sent to your email.";
        }
    }
}
