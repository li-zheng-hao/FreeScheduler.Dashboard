using System.Runtime.CompilerServices;
using FreeScheduler.Dashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FreeScheduler.Dashboard;

/// <summary>
/// FreeScheduler处理接口
/// </summary>
[ApiController]
[Route("scheduler/[controller]/[action]")]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;
    private readonly IFreeSql _fsql;

    public TaskController(ILogger<TaskController> logger, IFreeSql freesql)
    {
        _logger = logger;
        _fsql = freesql;
    }

    /// <summary>
    /// 定时任务分页查询
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> PageQueryTask([FromQuery] PageQueryParam param)
    {
        var page = _fsql.Select<dynamic>()
            .WithSql($"select * from {FreeSchedulerDashboardMiddlewareExtensions.option.TaskTableName} ")
            .WhereIf(string.IsNullOrWhiteSpace(param.filterKey) == false, " topic like @key",
                new { key = $"{param.filterKey}%" });
        var count = await page.CountAsync();
        var rows = await page.Page(param.pageNumber, param.pageSize)
            .OrderBy("last_run_time desc")
            .ToListAsync<dynamic>("*");
        return new { count, rows };
    }

    /// <summary>
    /// 定时任务日志分页查询
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> PageQueryTaskLog([FromQuery] TaskLogQueryParam param)
    {
        var page = _fsql.Select<dynamic>()
            .WithSql($"select * from {FreeSchedulerDashboardMiddlewareExtensions.option.TaskLogTableName} ")
            .WhereIf(param.success!=-1, " success = @success ",
                new { success=param.success})
            .WhereIf(string.IsNullOrWhiteSpace(param.filterKey)==false," task_id=@id",new{id=param.filterKey})
            .Page(param.pageNumber, param.pageSize);
        var count = await page.CountAsync();

        var rows = await page.OrderBy("create_time desc")
            .ToListAsync<dynamic>("*");
        return new { rows, count };
    }

    /// <summary>
    /// 更新任务状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> UpdateTaskState(UpdateTaskParam param)
    {
        var status = param.status switch
        {
            0 => "Running",
            1 => "Paused",
            2 => "Completed"
        };
        var rows = await _fsql.Update<dynamic>().AsTable(FreeSchedulerDashboardMiddlewareExtensions.option.TaskTableName)
            .SetRaw("status = @status", new {status })
            .Where("id=@task_id", new { param.task_id })
            .ExecuteAffrowsAsync();
        return rows > 0;
    }
}