using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendPasswordResetAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            var resetUrl = $"{_settings.ResetPasswordUrl}?token={Uri.EscapeDataString(token)}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Redefinição de senha";

            message.Body = new BodyBuilder
            {
                HtmlBody = $"""
                    <h2>Redefinição de senha</h2>
                    <p>Recebemos uma solicitação para redefinir sua senha.</p>
                    <p><a href="{resetUrl}">Clique aqui para redefinir sua senha</a></p>
                    <p>Este link expira em 30 minutos.</p>
                    <p>Se você não solicitou a redefinição, ignore este e-mail.</p>
                    """
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_settings.User, _settings.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

        public async Task SendEmailConfirmationAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            var confirmationUrl = $"{_settings.ConfirmEmailUrl}?token={Uri.EscapeDataString(token)}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Confirmação de e-mail";

            message.Body = new BodyBuilder
            {
                HtmlBody = $"""
            <h2>Confirmação de e-mail</h2>
            <p>Obrigado por criar sua conta.</p>
            <p>Confirme seu endereço de e-mail para concluir o cadastro.</p>
            <p><a href="{confirmationUrl}">Confirmar meu e-mail</a></p>
            <p>Este link expira em 24 horas.</p>
            <p>Se você não criou esta conta, ignore este e-mail.</p>
            """
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_settings.User, _settings.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
