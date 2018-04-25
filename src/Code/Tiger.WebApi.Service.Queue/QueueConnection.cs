using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System.IO;
using Tiger.WebApi.Core.Service.Extensions;

namespace Tiger.WebApi.Service.Queue
{
    public class QueueConnection
    {
        private static readonly JObject jObject = JObject.Parse(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "TigerWebApiConfig.json")));
        // 偷懒
        private static IConnection _conn = null;
        private QueueConnection() { }

        public static IConnection GetInstance()
        {
            if (_conn == null)
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    UserName = jObject.GetValue2("RebbitMQ:UserName"),
                    Password = jObject.GetValue2("RebbitMQ:Password"),
                    VirtualHost = jObject.GetValue2("RebbitMQ:VirtualHost"),
                    HostName = jObject.GetValue2("RebbitMQ:HostName")
                };
                _conn = factory.CreateConnection();
            }
            return _conn;
        }
    }
}
