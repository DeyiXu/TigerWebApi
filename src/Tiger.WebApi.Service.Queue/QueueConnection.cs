using RabbitMQ.Client;

namespace Tiger.WebApi.Service.Queue
{
    public class QueueConnection
    {
        // 偷懒
        private static IConnection _conn = null;
        private QueueConnection() { }

        public static IConnection GetInstance()
        {
            if (_conn == null)
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    HostName = "192.168.0.123"
                };
                _conn = factory.CreateConnection();
            }
            return _conn;
        }
    }
}
