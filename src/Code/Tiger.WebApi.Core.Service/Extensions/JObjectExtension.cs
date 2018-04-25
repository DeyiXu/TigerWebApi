using Newtonsoft.Json.Linq;

namespace Tiger.WebApi.Core.Service.Extensions
{
    public static class JObjectExtension
    {
        /// <summary>
        /// 获取Value
        /// </summary>
        /// <param name="jobj"></param>
        /// <param name="key">key1:key2:key3...</param>
        /// <returns></returns>
        public static string GetValue2(this JObject jobj, string key)
        {
            string[] keys = key.Split(':');
            JToken jToken = jobj.GetValue(keys[0]);
            for (int i = 1; i < keys.Length; i++)
            {
                jToken = jToken[keys[i]];
            }
            return jToken.ToString();
        }
    }
}
