using Gym.Application.Interfaces;
using MediatR;
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
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with {request.UserId}  not found.");

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
