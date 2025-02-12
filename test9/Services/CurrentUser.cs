using System.Security.Claims;

namespace InstagramDMs.API.Services;

public class CurrentUser
{
    private readonly ClaimsPrincipal? _user;
    private readonly bool _isAuthenticated;
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User;
        _isAuthenticated = httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    private CurrentUser(HttpContext context)
    {
        _user = context?.User;
        _isAuthenticated = context?.User?.Identity?.IsAuthenticated ?? false;
    }

    public static CurrentUser Create(HttpContext context)
    {
        return new CurrentUser(context);
    }


    public Guid Id
    {
        get
        {
            if (!_isAuthenticated) return Guid.Empty;
            var claim = _user?.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim == null
                ? throw new Exception("Invalid user, user id missing in token.")
                : Guid.Parse(claim);
        }
    }

    public Guid BusinessId
    {
        get
        {
            if (!_isAuthenticated) return Guid.Empty;
            var claim = _user?.FindFirstValue("business_id");
            return claim == null
                ? throw new Exception("Invalid user, business id missing in token.")
                : Guid.Parse(claim);
        }
    }
}
