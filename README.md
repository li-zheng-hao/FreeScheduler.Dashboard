# FreeScheduler.Dashboard
Freescheduler监控面板

# 使用方式


```sh
Install-Package FreeScheduler.Dashboard # 安装nuget包
```

```c#
app.UseFreeSchedulerDashboard(it =>
{
    // it.DashboardAuthorizationFilter = null;// 自定义权限校验 不自定义的话默认本地地址才能访问
    // it.TaskTableName // 表名 默认FreeScheduler_task
    // it.TaskLogTableName // 日志表名 默认FreeScheduler_tasklog
});


app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
// .NET 6 把下面这行换成上面的
// app.MapControllers();
```

打开`http://ip地址:端口号/schedulerdashboard/index.html`即可访问到监控面板

截图如下：

![image-20220908180146843](README.assets/image-20220908180146843.png)

![image-20220909153649180](README.assets/image-20220909153649180.png)

# 注意事项

1. 使用的.NET 6版本编译的，其它低版本目前需要自己修改代码集成
2. 使用http访问如果存在https重定向需自行修改跨域配置，使用https访问则无此问题
3. 权限访问方面，可自行继承`IDashboardAuthorizationFilter`接口，根据返回值来决定是否有权限访问页面
