namespace API.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);
    }
}
