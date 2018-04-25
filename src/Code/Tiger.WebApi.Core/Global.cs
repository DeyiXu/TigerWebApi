using System;
using System.Reflection;
using Tiger.WebApi.Core.Collections.Generic;

namespace Tiger.WebApi.Core
{
	public static class Global
	{
		public static SyncDictionary<string, Type> Service = new SyncDictionary<string, Type>();
		public static SyncDictionary<string, PackageItem> PackageItem = new SyncDictionary<string, PackageItem>();
		public static SyncDictionary<string, string> App = new SyncDictionary<string, string>();
		public static SyncDictionary<string, Assembly> Assembly = new SyncDictionary<string, Assembly>();
	}
}
