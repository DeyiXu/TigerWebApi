using TigerWebApiDemo.Entities;

namespace TigerWebApiDemo.DAL
{
	public class UserDAL
	{
		public UserEntity Get(string userName)
		{
			return new UserEntity()
			{
				UserName = userName,
				Password = "rootpass",
			};
		}
	}
}
