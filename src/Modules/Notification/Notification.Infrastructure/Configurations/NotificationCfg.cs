namespace Notification.Infrastructure.Configurations;

public static class NotificationCfg
{
    public static class EmailSettings
    {
        public const string Section = "EmailSettings";

        public const string SmtpServer = "SmtpServer";

        public const string SmtpPort = "SmtpPort";

        public const string FromAddress = "FromAddress";

        public const string FromName = "FromName";

        public const string Username = "Username";

        public const string Password = "Password";

        public const string EnableSsl = "EnableSsl";

        public const string TimeoutMs = "TimeoutMs";
    }
}
