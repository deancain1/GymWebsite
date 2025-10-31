using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Domain.Interfaces
{
    public interface IQRCodeService
    {
        string GenerateQRCode(string content);
    }
}
