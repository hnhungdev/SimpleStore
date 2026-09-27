using Notification.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Notification.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Enums;

namespace Notification.Infrastructure.Persistence.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<NotificationDbContext>();

        await SeedDataAsync(dbContext);
    }

    private static async Task SeedDataAsync(NotificationDbContext dbContext)
    {
        if (await dbContext.NotificationTemplates.AnyAsync())
        {
            return;
        }

        var notificationTemplate1 = NotificationTemplateEntity.Create(
            key: TemplateKey.OrderConfirmed,
            channel: ChannelType.InApp,
            subject: "Order Confirmed",
            isHtml: false,
            body: "Your order #ORDER_NO# has been confirmed successfully. Total amount: #ORDER_TOTAL#.");


        var notificationTemplate2 = NotificationTemplateEntity.Create(
            key: TemplateKey.OrderConfirmed,
            channel: ChannelType.Email,
            subject: "Your order #ORDER_NO# has been confirmed",
            isHtml: true,
            body: """
                <h2>Order Confirmation</h2>

                    <p>Dear #CUSTOMER_NAME#,</p>

                        <p>
                            Thank you for shopping with us.
                            Your order <strong>#ORDER_NO#</strong> has been confirmed successfully.
                        </p>

                        <p>
                            Total Amount:
                            <strong>#ORDER_TOTAL#</strong>
                        </p>

                        <p>
                            You can track your order using the link below:
                        </p>

                        <p>
                            <a href="#ORDER_URL#">View Order</a>
                        </p>

                        <br/>

                    <p>Best regards,</p>
                <p>Ecommerce Team</p>
                """);

        dbContext.NotificationTemplates.AddRange(
            notificationTemplate1,
            notificationTemplate2);

        await dbContext.SaveChangesAsync();
    }
}
