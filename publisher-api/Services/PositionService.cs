using System;
using System.Text;
using RabbitMQ.Client;

namespace publisher_api.Services
{
    public interface IPositionService
    {
        bool Save(Position gps);
    }

    public class PositionService : IPositionService
    {
        private const string QUEUE_URL = "amqp://meetup:demo@127.0.0.1:5672";
        private const string QUEUE_NAME = "meetup";

        private IConnection _connection;
        private IModel _channel;

        public PositionService()
        {
            // configure RabbitMQ by creating a connection & channel
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(QUEUE_URL)
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _initQueue(_channel);
        }

        public bool Save(Position gps)
        {
            // serialize position to string
            var messageBytes = Encoding.UTF8.GetBytes(gps.ToString());

            // put message into queue
            var props = _channel.CreateBasicProperties();

            _channel.BasicPublish(exchange: "",
                routingKey: QUEUE_NAME,
                basicProperties: props,
                body: messageBytes);

            return true;
        }

        /// <summary>
        /// Creates a new RabbitQueue if one doesn't already exist.
        /// </summary>
        /// <param name="channel"></param>
        private void _initQueue(IModel channel)
        {
            var properties = channel.CreateBasicProperties();
            // persistent = true means the message will be written to disk before sending so we are protected from losing unprocessed messages on a restart.
            properties.Persistent = true;

            channel.QueueDeclare(queue: QUEUE_NAME,
                // durable means the queue will survive a restart of the RabbitMQ server
                durable: true,
                // exclusive = true means the queue is used by only one connection and will be deleted when the connection is closed
                exclusive: false,
                // autoDelete = true means the server will delete the queue when it is no longer in use
                autoDelete: false,
                arguments: null);
        }
    }
}
