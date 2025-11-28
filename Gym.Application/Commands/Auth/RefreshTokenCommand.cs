using Gym.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<TokenResponse>
    {
        public string Token { get; set; }          
        public string RefreshToken { get; set; }
    }
}
