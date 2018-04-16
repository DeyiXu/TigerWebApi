# TigerWebApi 是什么？
TigerWebApi 是一个热插拔的API，实现了项目不停止的情况下对项目API进行更新。

# 环境
>`.NET Standard 2.0`

>`ASP.NET Core 2.0`

# 介绍
`Tiger.WebApi.Core` 是API处理的核心文件

`Tiger.WebApi` 是基于ASP.NET Core 2.0 引用和配置了`Tiger.WebApi.Core`

`Tiger.WebApi.Client` 是使用TigerWebApi的请求封装

`Tiger.Test` 测试Api和Client使用到的，有使用TigerWebApiClient方法

`Tiger.Account` 处理和用户相关的业务，引用 `Tiger.WebApi.Core` 在类当中添加`Method`Attribute。继承`BaseMetchod`类或者实现`ITigerMethod`方法
# 使用方法 ASP.NET Core
1. 引用 `Tiger.WebApi.Core`
2. Startup > ConfigureServices 方法中添加
```cs
services.SettingsTigerWebApi();
```
3. Startup > Configure 
```cs
app.Map("/info", ApiHandler.Info);
app.Map("/rest", ApiHandler.Map);
```
4. 启动项目
5. 如🌰（例子）`Tiger.Account`,发布后`\bin\Debug\netstandard2.0`对文件进行重命名`Tiger.Account.dll|.pdb`>`Item.dll|.pdb`（*.deps.json可以删除）。

在WebApi下的Packages中新建任意文件夹可以以`Tiger.Account`命名，复制刚刚修改好的Item等其他文件到`Tiger.Account`文件夹中。

WebApi会自动加载,打开浏览器输入`http://localhost:5000/info`就能看到API信息。

`Tiger.Test` 项目中有封装好的调用方式
```cs
IDictionary<string, string> dic = new Dictionary<string, string>
{
    { "v", "v1" },
    { "k2", "k2" },
    { "k3", "k3" },
    { "k4", "k4" }
};

using (ITigerWebApiClient client = new DefaultTigerWebApiClient("http://localhost:5000/rest", "10000", "qwerasdfzxcv"))
{
    var content = client.Execute("tiger.service.account.getname", dic);
    Console.WriteLine(content);
}
```
# 请求协议
![请求协议](https://github.com/DeyiXu/TigerWebApi/images/headers.png)

![请求参数](https://github.com/DeyiXu/TigerWebApi/images/values.png)

# 开源协议
[MIT](https://github.com/DeyiXu/TigerWebApi/blob/master/LICENSE)
