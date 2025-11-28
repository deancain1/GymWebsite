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

namespace Gym.Application.Commands.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
         
            var user = await _tokenService.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null) throw new UnauthorizedAccessException("Invalid refresh token");

        
            var isValid = await _tokenService.ValidateRefreshTokenAsync(user, request.RefreshToken);
            if (!isValid) throw new UnauthorizedAccessException("Invalid or expired refresh token");

          
            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken =  _tokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = await _tokenService.GenerateAndSaveRefreshTokenAsync(user);

            return new TokenResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
