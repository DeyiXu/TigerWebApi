using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using Tiger.WebApi.Core.Constants;
using Tiger.WebApi.Core.IO;
using Tiger.WebApi.Core.Reflection;

namespace Tiger.WebApi.Core.Extensions
{
	public static class IServiceCollectionExtension
	{
		static Assembly TigerResolveEventHandler(object sender, ResolveEventArgs args)
		{
			string fileName = args.Name.Split(',')[0];
			string requestFileName = args.RequestingAssembly.ToString().Split(',')[0];
			string responseDir = Global.PackageItem[requestFileName].Directory;
			return Assembly.Load(File.ReadAllBytes(Path.Combine(responseDir, $"{fileName}.dll")));
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
					//
					Common.RemoveService(fullPath);
					break;
			}
			Console.WriteLine($"{type.ToString()}|{fullPath}");
		}

		public static IServiceCollection SettingsTigerWebApi(this IServiceCollection services)
		{
			#region 验证文件夹
			string packagePath = Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME);
			if (!Directory.Exists(packagePath))
				Directory.CreateDirectory(packagePath);
			#endregion

			#region 设置PackageItem
			PackageItem[] packageItems = AssemblyHelper.GetPackageItems(packagePath);

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
			Global.App.Add("10000", "qwerasdfzxcv");
			#endregion

			#region 自动引用依赖文件
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyResolve += new ResolveEventHandler(TigerResolveEventHandler);
			#endregion

			#region 监听文件
			FileWatcher fileWatcher = new FileWatcher(Path.Combine(Environment.CurrentDirectory, CommonConstant.PACKAGE_NAME));
			fileWatcher.Changed += FileWatcherChanged;
			fileWatcher.Run();
			#endregion

			return services;
		}
	}
}
