using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Tiger.WebApi.Client
{
    public class DefaultTigerWebApiClient : ITigerWebApiClient
    {
        internal string serverUrl;
        internal string appKey;
        internal string appSecret;
        internal HttpClient httpClient;
        public DefaultTigerWebApiClient(string serverUrl, string appKey, string appSecret)
        {
            this.serverUrl = serverUrl;
            this.appKey = appKey;
            this.appSecret = appSecret;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(Constants.REST_APP_KEY, appKey);
            httpClient.DefaultRequestHeaders.Add(Constants.REST_SIGN_METHOD, Constants.SIGN_METHOD_MD5);
            httpClient.DefaultRequestHeaders.Add(Constants.REST_PARTNER_ID, "TigerWebApiClient");
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="method">API方法</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public string Execute(string method, IDictionary<string, string> parameters)
        {   // 设置Headers
            httpClient.DefaultRequestHeaders.Add(Constants.REST_TIMESTAMP, DateTime.Now.ToString(Constants.DATE_TIME_FORMAT));
            httpClient.DefaultRequestHeaders.Add(Constants.REST_METHOD, method);
            string sign = SignTopRequest(parameters, appSecret, Constants.SIGN_METHOD_MD5);
            httpClient.DefaultRequestHeaders.Add(Constants.REST_SIGN, sign);

            HttpContent httpContent = new FormUrlEncodedContent(parameters);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            using (HttpResponseMessage response = httpClient.PostAsync(serverUrl, httpContent).Result)
            {
                if (!response.IsSuccessStatusCode)
                    return null;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="secret"></param>
        /// <param name="signMethod"></param>
        /// <returns></returns>
        private static string SignTopRequest(IDictionary<string, string> parameters, string secret, string signMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            if (Constants.SIGN_METHOD_MD5.Equals(signMethod))
            {
                query.Append(secret);
            }
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            if (Constants.SIGN_METHOD_HMAC.Equals(signMethod))
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }
            else
            {
                query.Append(secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        public string Execute(string method)
        {
            return Execute(method, new Dictionary<string, string>());
        }
    }
}
