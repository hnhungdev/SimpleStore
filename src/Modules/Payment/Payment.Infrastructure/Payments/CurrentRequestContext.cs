using Microsoft.AspNetCore.Http;
using Payment.Application.Abstractions;

namespace Payment.Infrastructure.Payments;

public sealed class CurrentRequestContext(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentRequestContext
{
    public string IpAddress
    {
        get
        {
            var context = httpContextAccessor.HttpContext;

            if (context is null)
                return "127.0.0.1";

            var forwardedFor = context.Request.Headers["X-Forwarded-For"]
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            var remoteIp = context.Connection.RemoteIpAddress;

            if (remoteIp is null)
                return "127.0.0.1";

            return remoteIp.MapToIPv4().ToString();
        }
    }
}