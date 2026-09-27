using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
}
