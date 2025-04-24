using System.Security.Cryptography.X509Certificates;

namespace EmailClientBase.Services
{
    public class EmailSender
    {
        private readonly IEmailService _emailService;
        private readonly CancellationToken _cancellationToken = new CancellationToken();

        public EmailSender(IEmailService emailService)
        {
            _emailService = emailService;
            _ = SendEmailsPeriodically();
        }

        public async Task SendEmailsPeriodically()
        {
            while(!_cancellationToken.IsCancellationRequested)
            {
                await _emailService.SendEmailAsync("martindkulev@gmail.com", "blalaalal", "lalalalalala");
                await Task.Delay(TimeSpan.FromDays(30));
            }
        }
    }
}
