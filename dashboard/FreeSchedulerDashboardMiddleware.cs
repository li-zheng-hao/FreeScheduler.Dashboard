using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.FileProviders;

namespace FreeScheduler.Dashboard;

public class UseFreeSchedulerDashboard
{
    private readonly RequestDelegate _next;
    private readonly IApplicationBuilder _app;

    public UseFreeSchedulerDashboard(RequestDelegate next, IApplicationBuilder app)
    {
        _next = next;
        var ii = typeof(UseFreeSchedulerDashboard).Assembly;
        _app = app;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        if (SchedulerDashBoardOption.Instance.DashboardAuthorizationFilter != null)
        {
            var authRes=SchedulerDashBoardOption.Instance.DashboardAuthorizationFilter.Authorize(context);
            if (authRes == false)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        else if (context.Connection.RemoteIpAddress.ToString() != "127.0.0.1" &&
            context.Connection.RemoteIpAddress.ToString() != "::1")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (context.Request.Path.Value.Contains("assets"))
            context.Request.Path = context.Request.Path.Value.Replace("assets", "schedulerdashboard/assets");
        if (context.Request.Path.Value.Contains("schedulerdashboard/api"))
            context.Request.Path = context.Request.Path.Value.Replace("/schedulerdashboard/api", "");


        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public class SchedulerDashBoardOption
{
    public static SchedulerDashBoardOption Instance;

    private SchedulerDashBoardOption()
    {
    }

    static SchedulerDashBoardOption()
    {
        Instance = new SchedulerDashBoardOption();
    }

    public string TaskTableName { get; set; } = "FreeScheduler_task";
    public string TaskLogTableName { get; set; } = "FreeScheduler_tasklog";
    
    public IDashboardAuthorizationFilter? DashboardAuthorizationFilter { get; set; }
}

public static class FreeSchedulerDashboardMiddlewareExtensions
{

    /// <summary>
    /// 初始化地址
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="optionAction"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseFreeSchedulerDashboard(
        this IApplicationBuilder builder, Action<SchedulerDashBoardOption> action = null)
    {
        action?.Invoke(SchedulerDashBoardOption.Instance);
        builder.UseMiddleware<UseFreeSchedulerDashboard>(builder);
        var dir = Path.GetDirectoryName(typeof(TaskController).Assembly.Location);
        builder.UseStaticFiles(new StaticFileOptions()
        {
            // FileProvider = new PhysicalFileProvider(Path.Combine(dir, "dashboard")),
            FileProvider = new EmbeddedFileProvider(typeof(UseFreeSchedulerDashboard).Assembly,
                "FreeScheduler.Dashboard.dashboard"),
            RequestPath = "/schedulerdashboard"
        });
        return builder;
    }
}