namespace FoodDelivery.Infrastructure.Mail;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}