namespace AspNetCore.StartupTemplate.FreeSchedulerDashboard.Model;

public class UpdateTaskParam
{
    public string? task_id { get; set; } = "";
    public int status { get; set; }
}