using System.Collections.Generic;
using Tiger.WebApi.Core.Attributes;

namespace Tiger.WebApi.Core.Result
{
    public class PackageInfo
    {
        public PackageItem Item { get; set; }
        public List<MethodAttribute> Service { get; set; } = new List<MethodAttribute>();
    }
}
