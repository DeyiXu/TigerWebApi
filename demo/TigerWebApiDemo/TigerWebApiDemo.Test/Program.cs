using System;
using System.Collections.Generic;

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
