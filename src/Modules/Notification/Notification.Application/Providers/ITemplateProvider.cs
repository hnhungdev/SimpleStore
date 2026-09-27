namespace Notification.Application.Providers;

public interface ITemplateProvider
{
    string Render(string template, IDictionary<string, object> data = default!);
}
