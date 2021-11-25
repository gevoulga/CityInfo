using System.Diagnostics;
using CityInfo.API.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private readonly ILogger<LocalMailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly MailServerOptions _mailServerOptions;


        public LocalMailService(ILogger<LocalMailService> logger, IConfiguration configuration,
            IOptions<MailServerOptions> options)
        {
            _logger = logger;
            _configuration = configuration;
            _mailServerOptions = options.Value;
        }

        public void Send(string subject, string message)
        {
            //Send email
            _logger.LogInformation(
                $"Mail from {_configuration["MailServer.MailSettings:MailFrom"]} to {_configuration["MailServer.MailSettings:MailTo"]} with Local mail server.\nSubject: ${subject}\nBody:{message}");
            Debug.WriteLine("Sending email via local mail server");
        }
    }
}