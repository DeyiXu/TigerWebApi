# C#使用示例

add [NuGet](https://www.nuget.org/packages/Tiger.WebApi.Client/)

```pm
Install-Package Tiger.WebApi.Client -Version 1.0.1-alpha
```

```cs

string tigerAppID = "10000";
string tigerAppSecret = "qwerasdfzxcv";
string tigerApiUrl:"http://localhost:5000/rest";

IDictionary<string, string> args = new Dictionary<string, string>
{
    { "v", "V1" },
    { "uname", "root" },
    { "pass", "pass" }
};

using (ITigerWebApiClient client = new DefaultTigerWebApiClient(tigerApiUrl, tigerAppID, tigerAppSecret))
{
    client.Execute("tiger.account.login", args);
}
```