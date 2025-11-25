using Gym.Client.Interfaces;
using Gym.Client.Security;
using Gym.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Gym.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
          
            services.AddScoped<CustomAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

         
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            return services;
        }
    }
}
