using Gym.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Features.ForgotPassword
{
    public class ResetPasswordCommand : IRequest<ResetPasswordResult>
    {
        public string Email { get; set; } = default!;
        public string OtpCode { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
