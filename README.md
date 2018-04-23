# TigerWebApi 是什么？
TigerWebApi 是一个热插拔的API，实现了项目不停止的情况下对项目API进行更新。

# 环境
>`.NET Standard 2.0`

>`ASP.NET Core 2.0`

# 介绍
`Tiger.WebApi.Core` 是API处理的核心文件

`Tiger.WebApi.Core.Service` 业务逻辑使用

`Tiger.WebApi` 是基于ASP.NET Core 2.0 引用和配置了`Tiger.WebApi.Core`

`Tiger.WebApi.Client` 是使用TigerWebApi的请求封装

`Tiger.Test` 测试Api和Client使用到的，有使用TigerWebApiClient方法

`Tiger.Account` 处理和用户相关的业务，引用 `Tiger.WebApi.Core.Service` 在类当中添加`Method`Attribute。继承`BaseMetchod`类或者实现`ITigerMethod`方法

# 使用方法 Docker
```sh
docker pull undesoft/tigerwebapi

docker run --name my-tigerwebapi -p 5000:80 -v /Users/kevin/DockerData/Packages:/app/Packages -d undesoft/tigerwebapi:latest
```

# 使用方法 编译Docker并使用
```sh
cd [path]/src

docker-compose build

docker run --name [my-tigerwebapi] -p [5000]:80 -v [/Users/kevin/DockerData/Packages]:/app/Packages -d undesoft/tigerwebapi:latest
```
`[path]`:项目路径

`[my-tigerwebapi]`:容器名字

`[5000]`:外部映射端口

`[/Users/kevin/DockerData/Packages]`:外部映射路径

注：使用时去掉[]

# 其他Docker例子
```sh
docker run --name my-tigerwebapi-1 -p 5001:80 -v /Users/kevin/DockerData/Packages:/app/Packages -d undesoft/tigerwebapi:latest

docker run --name my-tigerwebapi-2 -p 5002:80 -v /Users/kevin/DockerData/Packages:/app/Packages -d undesoft/tigerwebapi:latest

docker run --name my-tigerwebapi-3 -p 5003:80 -v /Users/kevin/DockerData/Packages:/app/Packages -d undesoft/tigerwebapi:latest
```
运行多个容器映射同一个目录，实现更新

使用Nginx为N个容器做负载均衡


# 使用方法 ASP.NET Core
1. 引用`Tiger.WebApi.Core`
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
![请求协议](https://github.com/DeyiXu/TigerWebApi/raw/master/images/headers.png)

![请求参数](https://github.com/DeyiXu/TigerWebApi/raw/master/images/values.png)

# 开源协议
[MIT](https://github.com/DeyiXu/TigerWebApi/blob/master/LICENSE)
