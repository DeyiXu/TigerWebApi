using System.Collections.Generic;
using Tiger.WebApi.Core.Service;
using Tiger.WebApi.Core.Service.Attributes;
using Tiger.WebApi.Core.Service.Extensions;
using TigerWebApiDemo.DAL;
using TigerWebApiDemo.Entities;

namespace TigerWebApiDemo.Account
{
	[Method("tiger.account.login", "用户登录")]
	public class Login : BaseMetchod
	{
		private readonly UserDAL _userDAL = new UserDAL();
		public Login(IDictionary<string, string> args) : base(args)
		{

		}

		public override object Invoke()
		{
			if (_args.IsNullOrEmpty())
			{
				return new
				{
					status = false,
					message = "参数内容不能为空"
				};
			}
			else
			{
				switch (_args["v"])
				{
					case "V1":
						return V1();
					default:
						return base.Invoke();
				}
			}
		}

		private object V1()
		{
			string pass = _args["pass"];
			var result = new
			{
				status = true,
				message = ""
			};
			UserEntity user = _userDAL.Get(_args["uname"]);
			if (user == null)
			{
				result = new
				{
					status = false,
					message = "用户名或密码错误"
				};
			}
			else if (!user.Password.Equals(pass))
			{
				result = new
				{
					status = false,
					message = "密码错误"
				};
			}
			return result;
		}
	}
}
