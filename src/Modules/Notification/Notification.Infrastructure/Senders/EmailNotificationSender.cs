using Microsoft.Extensions.Configuration;
using Notification.Application.Models;
using Notification.Domain.Enums;
using System.Net;
using System.Net.Mail;
using System.Text;
using Notification.Application.Strategy;
using Notification.Infrastructure.Configurations;

namespace Notification.Infrastructure.Senders;

public sealed class EmailNotificationSender : INotificationSender
{
    public ChannelType Channel => ChannelType.Email;

    private readonly string _smtpServer;

    private readonly int _smtpPort;

    private readonly string _fromAddress;

    private readonly string _fromName;

    private readonly string _username;

    private readonly string _password;

    private readonly bool _enableSsl;

    private readonly int _timeoutMs;

    public EmailNotificationSender(IConfiguration cfg)
    {
        _smtpServer = cfg.GetValue<string>($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.SmtpServer}")!;
        _smtpPort = cfg.GetValue($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.SmtpPort}", 587);
        _fromAddress = cfg.GetValue($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.FromAddress}", "admin@progcoder.com")!;
        _fromName = cfg.GetValue($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.FromName}", "ProG Coder")!;
        _username = cfg.GetValue<string>($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.Username}")!;
        _password = cfg.GetValue<string>($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.Password}")!;
        _enableSsl = cfg.GetValue($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.EnableSsl}", false);
        _timeoutMs = cfg.GetValue($"{NotificationCfg.EmailSettings.Section}:{NotificationCfg.EmailSettings.TimeoutMs}", 30000);
    }

    public async Task<ChannelResult> SendAsync(NotificationContext context, CancellationToken ct)
    {
        try
        {
            using var smtpClient = CreateSmtpClient();
            using var mailMessage = CreateMailMessage(context);

            await smtpClient.SendMailAsync(mailMessage);

            return ChannelResult.Success();
        }
        catch (Exception ex)
        {
            return ChannelResult.Failure(ex.Message);
        }
    }

    private SmtpClient CreateSmtpClient()
    {
        var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
        {
            EnableSsl = _enableSsl,
            Timeout = _timeoutMs
        };

        if (!string.IsNullOrWhiteSpace(_username) &&
            !string.IsNullOrWhiteSpace(_password))
        {
            smtpClient.Credentials = new NetworkCredential(_username, _password);
        }

        return smtpClient;
    }

    private MailMessage CreateMailMessage(NotificationContext context)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromAddress, _fromName),
            Subject = context.Subject,
            Body = context.Body,
            IsBodyHtml = context.IsHtml,
            BodyEncoding = Encoding.UTF8,
            SubjectEncoding = Encoding.UTF8
        };

        mailMessage.To.Add(new MailAddress(context.Recipient));
        
        return mailMessage;
    }
}
