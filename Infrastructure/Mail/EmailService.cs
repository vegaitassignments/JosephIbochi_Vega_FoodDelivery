
using System.Net.Mail;

namespace FoodDelivery.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpServer = _config["EmailSettings:SmtpServer"];
        var smtpPort = int.Parse(_config["EmailSettings:Port"]);
        var senderEmail = _config["EmailSettings:SenderEmail"];

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.Credentials = new System.Net.NetworkCredential();
            client.EnableSsl = false; // I disabled this since MailDev does not require SSL

            var message = new MailMessage(senderEmail, toEmail, subject, body);
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}