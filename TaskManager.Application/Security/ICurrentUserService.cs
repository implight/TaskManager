
using System.Security.Claims;

namespace TaskManager.Application.Security
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
        bool HasClaim(string type, string value);
        IEnumerable<string>? Roles { get; }
        IEnumerable<Claim>? Claims { get; }
    }
}
