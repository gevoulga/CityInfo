using System.Diagnostics;
using CityInfo.API.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private readonly ILogger<CloudMailService> _logger;
        private readonly MailServerOptions _mailServerOptions;

        public CloudMailService(ILogger<CloudMailService> logger, IOptions<MailServerOptions> options)
        {
            _logger = logger;
            _mailServerOptions = options.Value;
        }

        public void Send(string subject, string message)
        {
            // send mail - output to debug window
            _logger.LogInformation(
                $"Mail from {_mailServerOptions.MailSettings.MailFrom} to {_mailServerOptions.MailSettings.MailTo}, with CloudMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}