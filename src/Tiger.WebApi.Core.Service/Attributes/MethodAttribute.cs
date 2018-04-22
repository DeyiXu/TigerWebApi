using Newtonsoft.Json;
using System;

namespace Tiger.WebApi.Core.Service.Attributes
{
    /// <summary>
    /// 方法属性
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [AttributeUsage(AttributeTargets.Class)]
    public class MethodAttribute : Attribute
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string Description { get; private set; } = "";

        public MethodAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
