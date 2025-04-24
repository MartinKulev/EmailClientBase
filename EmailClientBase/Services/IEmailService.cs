using MimeKit;

namespace EmailClientBase.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

        Task<List<MimeMessage>> GetInboxAsync();
    }
}
