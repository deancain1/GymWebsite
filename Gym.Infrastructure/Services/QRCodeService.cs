using Gym.Domain.Interfaces;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Services
{
    public class QrCodeService : IQRCodeService
    {
        public string GenerateQRCode(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrData);

            return qrCode.GetGraphic(20); // returns Base64 string
        }
    }
}
