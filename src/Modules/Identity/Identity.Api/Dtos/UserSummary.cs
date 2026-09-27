namespace Identity.Api.Dtos;

public class UserSummary
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsLockedOut { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
}
