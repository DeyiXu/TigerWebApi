using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using Tiger.WebApi.Core.Constants;
using Tiger.WebApi.Core.IO;
using Tiger.WebApi.Core.Reflection;
using Tiger.WebApi.Core.Service.Extensions;

namespace Tiger.WebApi.Core.Extensions
{
    public static class IServiceCollectionExtension
    {
        static Assembly TigerResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string[] nameSplit = args.Name.Split(',');
            string fileName = nameSplit[0].Trim();
            string version = nameSplit[1].Replace("Version=", "").Trim();

            string filePath = Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME, CommonConstant.PACKAGE_COMMON_NAME, fileName, $"{fileName}-{version}.dll");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME, CommonConstant.PACKAGE_COMMON_NAME, fileName, $"{fileName}-1.0.0.0.dll");
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME, CommonConstant.PACKAGE_COMMON_NAME, fileName, $"{fileName}.dll");
                    if (!File.Exists(filePath))
                    {
                        throw new Exception($"未找到文件:{fileName}.dll");
                    }
                }
            }

            if (Global.Assembly.ContainsKey(filePath))
            {
                return Global.Assembly[filePath];
            }
            else
            {
                Global.Assembly.Add(filePath, Assembly.Load(File.ReadAllBytes(filePath)));
                return Global.Assembly[filePath];
            }
        }

        static void FileWatcherChanged(FileChangeType type, string fullPath)
        {
            PackageItem item = AssemblyHelper.GetPackageItem(fullPath);
            switch (type)
            {
                case FileChangeType.Update:
                    Common.RemoveService(fullPath);
                    Common.AddService(item);
                    // 更新类
                    break;
                case FileChangeType.Create:
                    Common.AddService(item);
                    // 添加类
                    break;
                case FileChangeType.Delete:
                    // 删除类
                    Common.RemoveService(fullPath);
                    break;
            }
        }

        public static IServiceCollection SettingsTigerWebApi(this IServiceCollection services)
        {
            #region 验证文件夹

            string packagePath = Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME);
            if (!Directory.Exists(packagePath))
                Directory.CreateDirectory(packagePath);

            string[] dirs = {
                CommonConstant.PACKAGE_COMMON_NAME,
                CommonConstant.PACKAGE_SERVICE_NAME,
                CommonConstant.PACKAGE_SERVER_NAME
            };

            for (int i = 0; i < dirs.Length; i++)
            {
                string dirPath = Path.Combine(packagePath, dirs[i]);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
            }
            #endregion

            #region 设置PackageItem
            PackageItem[] packageItems = AssemblyHelper.GetPackageItems(Path.Combine(packagePath, CommonConstant.PACKAGE_SERVICE_NAME));

            foreach (PackageItem packageItem in packageItems)
            {
                // 设置PackageItem(重复出现则会覆盖)
                Global.PackageItem[packageItem.AssemblyName] = packageItem;

                foreach (Type typeItem in packageItem.Assembly.GetExportedTypes())
                {
                    Common.AddService(typeItem);
                }
            }
            #endregion

            #region 设置APP，后期使用DB|Cache替换
            JObject config = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, CommonConstant.CONFIG_FILENAME)));
            Global.App.Add(config.GetValue2("Server:AppID"), config.GetValue2("Server:AppSecret"));
            #endregion

            #region 自动引用依赖文件
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += TigerResolveEventHandler;
            #endregion

            #region 监听文件
            string variable = Environment.GetEnvironmentVariable(CommonConstant.ENVIRONMENT_PACKAGE_WATCHER) ?? "";
            string packagesServicePath = Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME, CommonConstant.PACKAGE_SERVICE_NAME);
            IFileWatcher fileWatcher = null;
            if (variable.Equals("Thread"))
            {
                fileWatcher = new ThreadFileWatcher(packagesServicePath);
            }
            else
            {
                fileWatcher = new FileWatcher(packagesServicePath);
            }
            fileWatcher.Changed += FileWatcherChanged;
            fileWatcher.Run();
            #endregion

            return services;
        }
    }
}
