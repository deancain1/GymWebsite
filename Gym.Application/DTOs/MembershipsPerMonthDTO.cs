using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.DTOs
{
    public class MembershipsPerMonthDTO
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
