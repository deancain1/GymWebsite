using Gym.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces
{
    public interface IPasswordRepostitory
    {
        Task AddAsync(PasswordResetOTP otp);
        Task<PasswordResetOTP?> GetValidOTPAsync(string userId, string otpCode);
        Task SaveAsync();
    }
}