using System;
using System.Collections.Generic;
using Tiger.WebApi.Client;

namespace Tiger.Test
{
    class Program
    {
        static void Main(string[] args)
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
            Console.ReadKey();
        }
    }
}
