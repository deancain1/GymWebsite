using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdateUserCommandHandler(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with {request.UserId}  not found.");



            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                
                var emailResult = await _userManager.SetEmailAsync(user, request.Email);
                if (!emailResult.Succeeded)
                    throw new Exception(string.Join("; ", emailResult.Errors.Select(e => e.Description)));

                
                var userNameResult = await _userManager.SetUserNameAsync(user, request.Email);
                if (!userNameResult.Succeeded)
                    throw new Exception(string.Join("; ", userNameResult.Errors.Select(e => e.Description)));
            }

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword!);

                if (!passResult.Succeeded)
                    throw new Exception(string.Join("; ", passResult.Errors.Select(e => e.Description)));
            }

            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;
            user.Address = request.Address;
            user.ProfilePicture = request.ProfilePicture;
            await _userRepository.UpdateUserAsync(user);


        }
    }
}
