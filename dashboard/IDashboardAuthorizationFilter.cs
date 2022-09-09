using Microsoft.AspNetCore.Http;

namespace FreeScheduler.Dashboard;

public interface IDashboardAuthorizationFilter
{
    bool Authorize(HttpContext context);
}