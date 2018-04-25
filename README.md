# TigerWebApi 是什么？
TigerWebApi 是一个热插拔的API，实现了项目不停止的情况下对项目API进行更新。

# 环境
>`.NET Standard 2.0`

>`ASP.NET Core 2.0`

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
运行多个容器映射同一个目录,实现更新

使用Nginx为N个容器做负载均衡


# 使用方法 ASP.NET Core
1. 引用 [Tiger.WebApi.Core](https://www.nuget.org/packages/Tiger.WebApi.Core/)
2. Startup > ConfigureServices 方法中添加
```cs
services.SettingsTigerWebApi();
```
3. Startup > Configure 
```cs
app.Map("/info", ApiHandler.Info);
app.Map("/rest", ApiHandler.Map);
```
# 配置介绍
项目目录下的 `TigerWebApiConfig.json` 设置appKey和appSecret

在项目根目录下有一个`Packages`文件夹，文件夹中有三个文件夹`Common`、`Server`、`Service`

按照顺序发布`Common`=>`Server` / `Common`=>`Service`

如：有以下几个已发布文件
`Newtonsoft.Json.dll`

`Tiger.WebApi.Core.Service.dll`

`TigerWebApiDemo.IDAL.dll`

`TigerWebApiDemo.DAL.dll`

`TigerWebApiDemo.Entities.dll`

>`Common`:通用引用包

规则：

在新添加一引用包的时候需要根据包的`命名空间`(Newtonsoft.Json)来新建一个文件夹(Newtonsoft.Json)。
确保dll文件名要和文件夹名称一样，针对于不同的版本可以用`Newtonsoft.Json-2.0.0.0.dll`(命名空间-[版本号].dll)，在程序引用过程中会先去查找程序需要的版本dll文件，如果找不到。默认回去查找`Newtonsoft.Json-1.0.0.0.dll`版本的dll，还是找不到会去使用``Newtonsoft.Json.dll``，再找不到则程序会抛出异常信息。

`Tiger.WebApi.Core.Service.dll`

`TigerWebApiDemo.IDAL.dll`

`TigerWebApiDemo.DAL.dll`

`TigerWebApiDemo.Entities.dll`

则使用同样的方法

>`Server`:服务器引用包(针对于Api的升级)

针对于后期`Tiger.WebApi.Core`需要的功能

>`Service`:服务层引用包(也就是引用`Tiger.WebApi.Core.Service`)

复制已发布好的dll `TigerWebApiDemo.Account.dll`到当前目录下即可

# 客户端使用示例
[C#](https://github.com/DeyiXu/TigerWebApi/tree/master/src/Client/CSharp)

[微信小程序](https://github.com/DeyiXu/TigerWebApi/tree/master/src/Client/WeChat)

## 请求协议
![请求协议](https://github.com/DeyiXu/TigerWebApi/raw/master/images/headers.png)

![请求参数](https://github.com/DeyiXu/TigerWebApi/raw/master/images/values.png)

# 开源协议
[MIT](https://github.com/DeyiXu/TigerWebApi/blob/master/LICENSE)
