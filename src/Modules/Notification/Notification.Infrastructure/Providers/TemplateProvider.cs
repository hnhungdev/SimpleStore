using Notification.Application.Providers;
using System.Text.RegularExpressions;

namespace Notification.Infrastructure.Providers;

public sealed class TemplateProvider : ITemplateProvider
{
    private static readonly Regex Pattern = new(@"#(\w+)#", RegexOptions.Compiled);

    public string Render(string template, IDictionary<string, object> data = default!)
    {
        if (data == null) return template;

        return Pattern.Replace(template, m =>
        {
            var key = m.Groups[1].Value;
            return data.TryGetValue(key, out var val) && val is not null
            ? val.ToString()!
            : string.Empty;
        });
    }
}
