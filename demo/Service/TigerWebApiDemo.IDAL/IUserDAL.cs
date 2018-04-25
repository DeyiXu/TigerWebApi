using TigerWebApiDemo.Entities;

namespace TigerWebApiDemo.IDAL
{
    public interface IUserDAL
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserEntity Get(string userName);
    }
}
