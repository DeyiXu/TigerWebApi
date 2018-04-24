using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using Tiger.WebApi.Core.Service;
using Tiger.WebApi.Core.Service.Attributes;
using Tiger.WebApi.Core.Service.Extensions;

namespace Tiger.WebApi.Service.Queue
{
    [Method("tiger.webapi.service.queue.push", "消息入队")]
    public class Push : BaseMetchod
    {
        readonly IConnection _conn = null;
        const string QUEUE_KEY = "tiger.webapi.service.queue";
        public Push(IDictionary<string, string> args) : base(args)
        {
            _conn = QueueConnection.GetInstance();
        }

        public override object Invoke()
        {
            if (_args.IsNullOrEmpty())
            {
                return false;
            }
            using (IModel channel = _conn.CreateModel())
            {
                channel.QueueDeclareNoWait(QUEUE_KEY, true, false, false, null);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_args));
                channel.BasicPublish(exchange: "",
                                     routingKey: QUEUE_KEY,
                                     basicProperties: null,
                                     body: body);
            }
            return true;
        }
    }
}
