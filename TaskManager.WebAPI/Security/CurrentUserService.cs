
using System.Security.Claims;
using TaskManager.Application.Security;

namespace TaskManager.WebAPI.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Guid.TryParse(userIdClaim, out var userId))
                    return userId;

                return null;
            }
        }

        public string? UserName => _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.Name)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?
            .Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?
                .IsInRole(role) ?? false;
        }

        public bool HasClaim(string type, string value)
        {
            return _httpContextAccessor.HttpContext?.User?
                .HasClaim(type, value) ?? false;
        }

        public IEnumerable<Claim>? Claims
            => _httpContextAccessor.HttpContext?.User?.Claims;

        public IEnumerable<string>? Roles => _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value);
    }
}
