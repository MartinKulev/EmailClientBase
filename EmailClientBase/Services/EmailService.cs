using MailKit.Search;
using MailKit.Security;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;

namespace EmailClientBase.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _email;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _email = configuration["EmailSettings:Email"];
            _password = configuration["EmailSettings:Password"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", _email));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_email, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task<List<MimeMessage>> GetInboxAsync()
        {
            using var client = new ImapClient();
            await client.ConnectAsync("imap.gmail.com", 993, true);
            await client.AuthenticateAsync(_email, _password);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            var messages = new List<MimeMessage>();
            foreach (var uid in await inbox.SearchAsync(SearchQuery.All))
            {
                messages.Add(await inbox.GetMessageAsync(uid));
            }

            await client.DisconnectAsync(true);
            return messages;
        }
    }
}
