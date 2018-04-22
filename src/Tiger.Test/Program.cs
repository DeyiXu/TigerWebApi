using System;
using System.Collections.Generic;
using Tiger.WebApi.Client;
using Tiger.WebApi.Core.Service;

namespace Tiger.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestWebAPiServiceQueue();

            Console.ReadKey();
        }

        /// <summary>
        /// 测试Api客户端
        /// </summary>
        static void TestWebApiClient()
        {
            IDictionary<string, string> dic = new Dictionary<string, string>
            {
                { "v", "v1" },
                { "k2", "k2" },
                { "k3", "k3" },
                { "k4", "k4" }
            };

            using (ITigerWebApiClient client = new DefaultTigerWebApiClient("http://localhost:53613/rest", "10000", "qwerasdfzxcv"))
            {
                var content = client.Execute("tiger.service.account.getname", dic);
                Console.WriteLine(content);
            }
        }
        /// <summary>
        /// 测试消息列队
        /// </summary>
        static void TestWebAPiServiceQueue()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    IDictionary<string, string> dic = new Dictionary<string, string>
            //{
            //    { "v", "v1"+i },
            //    { "k2", "k"+i },
            //    { "k3", "k"+i },
            //    { "k4", "k"+i }
            //};
            //    ITigerMethod method = new WebApi.Service.Queue.Push(dic);
            //    method.Invoke();
            //}

            ITigerMethod method = new WebApi.Service.Queue.Pull(null);
            Console.WriteLine(method.Invoke());

            //using (IModel channel = _conn.CreateModel())
            //{
            //    BasicGetResult result = channel.BasicGet("tiger.webapi.service.queue", true);
            //    if (result == null)
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        byte[] bytes = result.Body;
            //        return Encoding.UTF8.GetString(bytes);
            //    }
            //}
        }
    }
}
