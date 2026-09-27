using Identity.Api.Dtos;

namespace Identity.Api.Services;

public interface IIdentityService
{
    Task<LoginResponse?> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task<LoginResponse?> IssueTokensAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserInfo?> GetUserInfoAsync(Guid userId, CancellationToken cancellationToken = default);
}
