using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        public readonly IConfiguration _config;
        public TokenService( IConfiguration config)
        {
            _config = config;
        }
        public string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Name, user.FullName),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim("PhoneNumber", user.PhoneNumber ?? string.Empty),
               new Claim("Address", user.Address ?? string.Empty),
                new Claim("FullName", user.FullName),
            };


            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

    
    }
}
