using System;
using System.Collections.Generic;
using System.Text;
using Tiger.WebApi.Core.Collections.Generic;

namespace Tiger.WebApi.Core
{
    public static class Global
    {
        //public static object ServiceLocker = new object();
        public static SyncDictionary<string, Type> Service = new SyncDictionary<string, Type>();
        //public static object PackageItemLocker = new object();
        public static SyncDictionary<string, PackageItem> PackageItem = new SyncDictionary<string, PackageItem>();
        public static SyncDictionary<string, string> App = new SyncDictionary<string, string>();
    }
}
