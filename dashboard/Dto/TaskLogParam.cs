namespace AspNetCore.StartupTemplate.FreeSchedulerDashboard.Model;

public class TaskLogQueryParam:PageQueryParam
{
    /// <summary>
    /// -1表示不适用
    /// </summary>
    public int success { get; set; } = -1;
}