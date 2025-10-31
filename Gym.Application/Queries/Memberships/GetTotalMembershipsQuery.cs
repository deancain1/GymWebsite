using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Memberships
{
    public class GetTotalMembershipsQuery : IRequest<int>
    {
    }
}
