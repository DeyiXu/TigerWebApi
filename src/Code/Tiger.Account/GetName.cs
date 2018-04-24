using System.Collections.Generic;
using Tiger.DAL;
using Tiger.WebApi.Core.Service;
using Tiger.WebApi.Core.Service.Attributes;

namespace Tiger.Service
{
    [Method("tiger.service.account.getname", "获取用户名称")]
    public class GetName : BaseMetchod
    {
        private readonly UserDAL _userDAL = new UserDAL();
        public GetName(IDictionary<string, string> parameters) : base(parameters)
        {

        }

        public override object Invoke()
        {
            string v = _args["v"];
            object result = null;
            switch (v)
            {
                case "v1":
                    result = V_1();
                    break;
                default:
                    result = base.Invoke();
                    break;
            }
            return result;
        }
        private object V_1()
        {
            return new
            {
                name = _userDAL.GetUserName(),
                value = "value"
            };
        }
    }
}
