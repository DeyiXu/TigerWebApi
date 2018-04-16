using System;
using System.Collections.Generic;

namespace Tiger.WebApi.Client
{
    public interface ITigerWebApiClient : IDisposable
    {
        /// <summary>
        /// 执行API
        /// </summary>
        /// <param name="method">API接口名称</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>API结果</returns>
        string Execute(string method, IDictionary<string, string> parameters);
        /// <summary>
        /// 执行API
        /// </summary>
        /// <param name="method">API接口名称</param>
        /// <returns></returns>
        string Execute(string method);
    }
}
