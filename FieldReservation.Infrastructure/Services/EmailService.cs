using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using FieldReservation.Domain.Exceptions;
using FieldReservation.Application.Interfaces;
using FieldReservation.Infrastructure.Settings;

namespace FieldReservation.Infrastructure.Services
{
    public class EmailService(IOptions<SmtpSettings> _options)
        : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body,
            CancellationToken cancellationToken = default)
        {
            SmtpSettings smtpSettings = _options.Value;

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(toEmail));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

            if (string.IsNullOrWhiteSpace(smtpSettings.UserName) || string.IsNullOrWhiteSpace(smtpSettings.Password))
            {
                Console.WriteLine($"[Mock Email] Sent to {toEmail} with subject: {subject}");
                return;
            }

            using SmtpClient client = new SmtpClient();

            try
            {
                await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
                await client.AuthenticateAsync(smtpSettings.UserName, smtpSettings.Password, cancellationToken);
                await client.SendAsync(mimeMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new EmailSendingException($"Failed to send email:{ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}