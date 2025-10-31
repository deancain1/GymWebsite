using Gym.Application.Interfaces;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Repository;
using Gym.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IQRCodeService, QrCodeService>();
            services.AddHostedService<MembershipExpirationService>();

            return services;
        }
    }
}
