using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Configuration;
using Streckenbuch.Server.Data.Entities;
using System.Text;

namespace Streckenbuch.Server.Services
{
    public class EmailSender : IEmailSender<ApplicationUser>
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IServiceProvider _provider;
        private readonly MailConfiguration _configuration;
        private readonly WebsiteConfiguration _siteConfiguration;

        public EmailSender(ILogger<EmailSender> logger, MailConfiguration configuration, WebsiteConfiguration siteConfiguration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _siteConfiguration = siteConfiguration;
            _provider = serviceProvider;

            logger.LogError("Init");
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("<p>Dear {0}</p>", user.Email));
            builder.AppendLine("<br>");

            string websiteName = _siteConfiguration.Name ?? "Website";
            builder.AppendLine($"<p>You've created an account at our Website \"{websiteName}\". To use the account you have to confirm your account</p>");
            builder.AppendLine("<p>To confirm your account, please open the link provided in this email</p>");

            builder.AppendLine("<br>");
            builder.AppendLine($"<a href=\"{confirmationLink}\">Click here</a>");
            builder.AppendLine("<br>");

            builder.AppendLine("<p>Kind Regards</p>");
            builder.AppendLine($"<p>{websiteName} Team</p>");

            await SendEmailAsync(email, "Confirm Account", builder.ToString());
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("<p>Dear {0}</p>", user.Email));
            builder.AppendLine("<br>");

            string websiteName = _siteConfiguration.Name ?? "Website";
            builder.AppendLine($"<p>You've stated that you forgot your password at \"{websiteName}\". To reset your password you've to confirm your identity</p>");
            builder.AppendLine("<p>To confirm your account, please open the link provided in this email</p>");

            builder.AppendLine("<br>");
            builder.AppendLine($"<a href=\"{resetLink}\">Click here</a>");
            builder.AppendLine("<br>");

            builder.AppendLine("<p>Kind Regards</p>");
            builder.AppendLine($"<p>{websiteName} Team</p>");

            await SendEmailAsync(email, "Forgotten Password Link", builder.ToString());
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            return Task.CompletedTask;
        }

        private async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(_configuration.Server))
            {
                throw new Exception("Mail Settings not configured!");
            }

            _logger.LogInformation("Mail Server: {0}", _configuration.Server);
            _logger.LogInformation("Mail Port: {0}", _configuration.Port);
            _logger.LogInformation("SSL: {0}", _configuration.SSL);
            _logger.LogInformation("Username: {0}", _configuration.Username);
            _logger.LogInformation("Password: {0}", _configuration.Password);

            using IServiceScope scope = _provider.CreateScope();

            var response = await scope.ServiceProvider.GetRequiredService<IFluentEmail>()
                .To(toEmail)
                .Subject(subject)
                .Body(message, true)
                .SendAsync();

            if (!response.Successful)
            {
                _logger.LogError("While sending the mail to {0} the following errors occured: {1}", toEmail, string.Join(',', response.ErrorMessages));
            }

            _logger.LogInformation(response.Successful ?
                "Email queued successfully!" :
                "Email couldn't be sent");
        }
    }
}
