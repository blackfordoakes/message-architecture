using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace position_consumer
{
    class Program
    {
        private const string QUEUE_URL = "amqp://meetup:demo@127.0.0.1:5672";
        private const string QUEUE_NAME = "meetup";

        private static IConnection _connection;
        private static IModel _channel;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting up. Press any key to exit.");
            _initRabbit();

            Console.WriteLine("Waiting for messages...");
            // wire up event to listen for messages
            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: QUEUE_NAME,
                autoAck: false,
                consumer: consumer);

            //TODO: multithreading
            consumer.Received += (model, ea) => {
                _doWork(model, ea);
            };

            // run continuously
            Console.ReadLine();
        }

        private static void _doWork(object model, BasicDeliverEventArgs ea)
        {
            var replyProperties = _channel.CreateBasicProperties();
            replyProperties.CorrelationId = ea.BasicProperties.CorrelationId;
            replyProperties.ReplyTo = ea.BasicProperties.ReplyTo;

            try
            {
                // get the weather
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Position gps = JsonConvert.DeserializeObject<Position>(message);

                Console.WriteLine($"[{DateTime.Now}] GPS Position {gps.Latitude}, {gps.Longitude} reached on {gps.Timestamp}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.ToString()}");
            }
            finally
            {
                // respond to caller
                _channel.BasicAck(ea.DeliveryTag, false);
            }
        }

        private static void _initRabbit()
        {
            // configure RabbitMQ by creating a connection & channel
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(QUEUE_URL)
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // create queue if not already created
            var properties = _channel.CreateBasicProperties();
            // persistent = true means the message will be written to disk before sending so we are protected from losing unprocessed messages on a restart.
            properties.Persistent = true;

            _channel.QueueDeclare(queue: QUEUE_NAME,
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
