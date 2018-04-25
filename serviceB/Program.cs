using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace serviceB
{
    class Program
    {
        static void Main(string[] args) => listen();

        public static void listen()
        {
            Console.WriteLine("Listening...");

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";

            using (var connection = factory.CreateConnection()){
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "myQ", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = System.Text.Encoding.UTF8.GetString(body);
                        var name = message.Split(", ")[1];
                        if (string.IsNullOrWhiteSpace(name))
                        {
                             throw new ArgumentException("name", nameof(name));
                        }
                        Console.WriteLine("Hello {0}, I am your father!", name);
                    };
                    channel.BasicConsume(queue: "myQ", autoAck: true, consumer: consumer);
                    Console.ReadLine();
                }
            }
        }
    }
}
