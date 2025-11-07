using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Users
{
    public class DeleteUserCommand : IRequest
    {
        public string UserId { get; set; }
    }
}
