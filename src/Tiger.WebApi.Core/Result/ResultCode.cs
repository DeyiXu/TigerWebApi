using Tiger.WebApi.Core.Attributes;

namespace Tiger.WebApi.Core.Result
{
    public enum ResultCode
    {
        [ResultCodeMessage("Header错误")]
        HeaderError = 0,
        [ResultCodeMessage("")]
        Success = 1,
        [ResultCodeMessage("签名不正确")]
        SignError = 2,
        [ResultCodeMessage("API未找到")]
        MethodNotFound = 3,
        [ResultCodeMessage("请求超时")]
        Timestamp = 4,
        [ResultCodeMessage("时间戳格式错误")]
        TimeFormat = 5,
        [ResultCodeMessage("AppKey错误")]
        AppKeyNotFound = 6,
        [ResultCodeMessage("请求参数不完整")]
        RequestParameterIncomplete = 7,
        [ResultCodeMessage("接口请求出错")]
        MethodError = 8
    }
}
