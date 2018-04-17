namespace Tiger.WebApi.Core.Result
{
    public class ResultObject
    {
        public ResultCode Code { get; set; }
        public string Message { get; set; } = "";
        public object Data { get; set; } = "";
    }
}
