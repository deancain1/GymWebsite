using Gym.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Services
{
    public class MembershipExpirationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MembershipExpirationService> _logger;

        public MembershipExpirationService(IServiceProvider serviceProvider, ILogger<MembershipExpirationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Membership expiration service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var membershipRepo = scope.ServiceProvider.GetRequiredService<IMembershipRepository>();
                        var memberships = await membershipRepo.GetAllMembershipsAsync();

                        var now = DateTime.UtcNow;

                        foreach (var member in memberships)
                        {
                            if (member.Status == "Accepted" && member.ExpirationDate <= now)
                            {
                                member.Status = "Expired";
                                await membershipRepo.UpdateStatusAsync(member);
                                _logger.LogInformation($"Membership {member.MemberID} has expired.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking membership expirations.");
                }

                // Wait 1 hour before checking again (you can change this to daily)
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
