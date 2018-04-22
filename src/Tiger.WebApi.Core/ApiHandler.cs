using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tiger.WebApi.Core.Constants;
using Tiger.WebApi.Core.Extensions;
using Tiger.WebApi.Core.Reflection;
using Tiger.WebApi.Core.Result;
using Tiger.WebApi.Core.Service;
using Tiger.WebApi.Core.Service.Attributes;

namespace Tiger.WebApi.Core
{
    public class ApiHandler
    {
        readonly ITigerMethod _tigerMethod;
        HttpContext _httpContext;
        ApiHandler(ITigerMethod tigerMethod, HttpContext httpContext)
        {
            this._tigerMethod = tigerMethod;
            this._httpContext = httpContext;
        }
        async Task EchoLoop()
        {
            _httpContext.Response.ContentType = CommonConstant.CONTENT_TYPE;
            ResultObject resultObject = new ResultObject();
            try
            {
                resultObject.Data = _tigerMethod.Invoke();
                resultObject.Code = ResultCode.Success;
                resultObject.Message = ResultCode.Success.ToMessage();
            }
            catch (Exception ex)
            {
                resultObject.Code = ResultCode.MethodError;
                resultObject.Message = ResultCode.MethodError.ToMessage();
                resultObject.Data = ex.Message;
            }

            await _httpContext.Response.WriteAsync(JsonConvert.SerializeObject(resultObject), Encoding.UTF8);
        }

        static bool RequestParameterVerification(IHeaderDictionary headers, IDictionary<string, string> args, out ResultCode resultCode)
        {
            string restMethod = headers[HeaderKeyConstant.REST_METHOD];
            string restAppKey = headers[HeaderKeyConstant.REST_APP_KEY];
            string restSignMethod = headers[HeaderKeyConstant.REST_SIGN_METHOD];
            string restSign = headers[HeaderKeyConstant.REST_SIGN];
            string restTimestamp = headers[HeaderKeyConstant.REST_TIMESTAMP];
            #region 验证必填
            if (string.IsNullOrEmpty(restMethod) ||
                string.IsNullOrEmpty(restAppKey) ||
                string.IsNullOrEmpty(restSignMethod) ||
                string.IsNullOrEmpty(restSign) ||
                string.IsNullOrEmpty(restTimestamp)
                )
            {
                resultCode = ResultCode.RequestParameterIncomplete;
                return false;
            }
            #endregion

            #region 验证APPKey
            if (!Global.App.ContainsKey(restAppKey))
            {
                resultCode = ResultCode.AppKeyNotFound;
                return false;
            }
            string appSecret = Global.App[restAppKey];
            #endregion

            #region 验证时间戳
            DateTime timestamp = DateTime.MinValue;
            if (!DateTime.TryParseExact(restTimestamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out timestamp))
            {
                resultCode = ResultCode.TimeFormat;
                return false;
            }
            DateTime maxTime = DateTime.Now;
            DateTime minTime = maxTime.AddMinutes(-1);
            if (timestamp < minTime || timestamp > maxTime)
            {
                resultCode = ResultCode.Timestamp;
                return false;
            }
            #endregion

            #region 验证API方法
            if (!Global.Service.ContainsKey(restMethod))
            {
                resultCode = ResultCode.MethodNotFound;
                return false;
            }
            #endregion

            #region 验证签名
            if (CommonConstant.SIGN_METHOD_MD5.Equals(restSignMethod) || CommonConstant.SIGN_METHOD_HMAC.Equals(restSignMethod))
            {
                string signNew = Common.BuildSign(args, appSecret, restSignMethod);
                if (!signNew.Equals(restSign))
                {
                    resultCode = ResultCode.SignError;
                    return false;
                }
            }
            else
            {
                resultCode = ResultCode.SignError;
                return false;
            }
            #endregion
            resultCode = ResultCode.Success;
            return true;
        }
        static async Task AcceptInfo(HttpContext hc, Func<Task> n)
        {
            List<PackageInfo> packageInfos = new List<PackageInfo>();
            foreach (var item in Global.PackageItem)
            {
                PackageInfo packageInfo = new PackageInfo
                {
                    Item = item.Value
                };
                foreach (Type type in item.Value.Assembly.GetExportedTypes())
                {
                    MethodAttribute attr = (MethodAttribute)type.GetCustomAttribute(typeof(MethodAttribute));
                    if (attr != null)
                    {
                        packageInfo.Service.Add(attr);
                    }
                }
                packageInfos.Add(packageInfo);
            }

            hc.Response.ContentType = CommonConstant.CONTENT_TYPE;
            await hc.Response.WriteAsync(JsonConvert.SerializeObject(packageInfos), Encoding.UTF8);
        }
        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            // 排除WebSockets
            if (hc.WebSockets.IsWebSocketRequest)
                return;
            // 只接受GET/POST
            if (hc.Request.Method != HttpMethods.Get && hc.Request.Method != HttpMethods.Post)
                return;

            IDictionary<string, string> args = new Dictionary<string, string>();
            if (hc.Request.Method == HttpMethods.Post)
            {
                if (hc.Request.Form != null)
                {
                    foreach (var item in hc.Request.Form)
                    {
                        args.Add(item.Key, item.Value);
                    }
                }
            }
            else
            {
                foreach (var item in hc.Request.Query)
                {
                    args.Add(item.Key, item.Value);
                }
            }

            if (!RequestParameterVerification(hc.Request.Headers, args, out ResultCode resultCode))
            {
                hc.Response.ContentType =CommonConstant.CONTENT_TYPE;

                ResultObject resultObject = new ResultObject()
                {
                    Code = resultCode,
                    Message = resultCode.ToMessage()
                };
                await hc.Response.WriteAsync(JsonConvert.SerializeObject(resultObject), Encoding.UTF8);
            }
            else
            {
                string method = hc.Request.Headers[HeaderKeyConstant.REST_METHOD];
                if (string.IsNullOrEmpty(method))
                    return;

                ITigerMethod tigerMethod = AssemblyHelper.CreateInstance(method, args);
                var h = new ApiHandler(tigerMethod, hc);
                await h.EchoLoop();
            }
        }

        public static void Map(IApplicationBuilder app)
        {
            app.Use(ApiHandler.Acceptor);
        }

        public static void Info(IApplicationBuilder app)
        {
            app.Use(ApiHandler.AcceptInfo);
        }
    }
}
