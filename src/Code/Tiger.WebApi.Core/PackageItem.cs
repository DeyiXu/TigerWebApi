using Newtonsoft.Json;
using System.Reflection;

namespace Tiger.WebApi.Core
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PackageItem
    {
        public Assembly Assembly { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string AssemblyName { get; set; }
        public string FileFullPath { get; set; }
        [JsonProperty]
        public string Description { get; set; }
    }
}
