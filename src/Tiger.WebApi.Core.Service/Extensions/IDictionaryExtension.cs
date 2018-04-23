using System.Collections.Generic;

namespace Tiger.WebApi.Core.Service.Extensions
{
    public static class IDictionaryExtension
    {
        public static bool IsNullOrEmpty(this IDictionary<string, string> dic)
        {
            if (dic == null || dic.Count == 0)
                return true;
            else
                return false;
        }
    }
}
