using System;
using TigerWebApiDemo.Entities;
using TigerWebApiDemo.IDAL;

namespace TigerWebApiDemo.DAL
{
    public class UserDAL : IUserDAL
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
