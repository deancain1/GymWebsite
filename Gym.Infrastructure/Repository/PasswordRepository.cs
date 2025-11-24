using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Repository
{
    public class PasswordRepository : IPasswordRepostitory
    {
        private readonly AppDbContext _context;
        public PasswordRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PasswordResetOTP otp)
        {
            await _context.PasswordResetOTP.AddAsync(otp);
        }

        public async Task<PasswordResetOTP?> GetValidOTPAsync(string userId, string otpCode)
        {
            return await _context.PasswordResetOTP
               .Where(x => x.UserId == userId
                           && x.OTP == otpCode
                           && !x.IsUsed
                           && x.ExpirationTime > DateTime.UtcNow)
               .FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
