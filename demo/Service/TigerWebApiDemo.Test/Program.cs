using System;
using System.Collections.Generic;
using Tiger.WebApi.Client;

namespace TigerWebApiDemo.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestLogin();
            Console.ReadKey();
        }
        static void TestWebApi()
        {
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "v", "V1" },
                { "uname", "root" },
                { "pass", "pass" }
            };
            using (ITigerWebApiClient client = new DefaultTigerWebApiClient("http://sss/rest", "", ""))
            {
                client.Execute("tiger.account.login", args);
            }
        }
        static void TestLogin()
        {
            IDictionary<string, string> args = new Dictionary<string, string>
            {
                { "v", "V1" },
                { "uname", "root" },
                { "pass", "pass" }
            };

            var result = new TigerWebApiDemo.Account.Login(args).Invoke();

        }
    }
}
