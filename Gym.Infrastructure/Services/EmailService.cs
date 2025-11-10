using Gym.Application.Interfaces;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemTask = System.Threading.Tasks.Task;

namespace Gym.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;

        public EmailService(string apiKey)
        {
            _apiKey = apiKey;
            Configuration.Default.ApiKey["api-key"] = _apiKey;
        }
        public async SystemTask SendEmailAsync(string to, string subject, string htmlContent)
        {
            var api = new TransactionalEmailsApi();

            var email = new SendSmtpEmail(
                sender: new SendSmtpEmailSender("Power Gym", "deanronquillo66@gmail.com"),
                to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(to) },
                subject: subject,
                htmlContent: htmlContent
            );

            await api.SendTransacEmailAsync(email);
        }
    }
}
