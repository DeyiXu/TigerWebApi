using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tiger.WebApi.Core.Constants;
using Tiger.WebApi.Core.Service;

namespace Tiger.WebApi.Core.Reflection
{
	public sealed class AssemblyHelper
	{
		public static string GetDescription(Assembly assembly)
		{
			AssemblyDescriptionAttribute asmdis = (AssemblyDescriptionAttribute)assembly.GetCustomAttribute(typeof(AssemblyDescriptionAttribute));
			if (asmdis == null)
				return "";

			return asmdis.Description ?? "";
		}
		public static string GetAssemblyName(Assembly assembly)
		{
			return assembly.ToString().Split(',')[0];
		}
		public static PackageItem GetPackageItem(string fileFullName, bool exists = false)
		{
			if (!exists)
				if (!File.Exists(fileFullName))
				{
					return null;
				}

			FileInfo fileInfo = new FileInfo(fileFullName);
			PackageItem packageItem = new PackageItem()
			{
				Name = fileInfo.Directory.Name,
				FileFullPath = fileFullName,
				Directory = fileInfo.Directory.FullName,
				Assembly = Assembly.Load(File.ReadAllBytes(fileFullName))
			};
			packageItem.AssemblyName = GetAssemblyName(packageItem.Assembly);
			packageItem.Description = GetDescription(packageItem.Assembly);
			return packageItem;
		}
		public static PackageItem[] GetPackageItems(string dir)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(dir);
			DirectoryInfo[] subDirs = dirInfo.GetDirectories();
			List<PackageItem> packageItems = new List<PackageItem>();
			foreach (var dirItem in subDirs)
			{
				string fileFullName = Path.Combine(dirItem.FullName, CommonConstant.PACKAGE_ITEM);
				if (File.Exists(fileFullName))
				{
					packageItems.Add(GetPackageItem(fileFullName, true));
				}
			}
			return packageItems.ToArray();
		}

		public static ITigerMethod CreateInstance(string method, IDictionary<string, string> args)
		{
			object obj = Activator.CreateInstance(Global.Service[method], new object[] { args });
			return obj as ITigerMethod;
		}
	}
}
