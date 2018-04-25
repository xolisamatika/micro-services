using System;
using System.Text;
using RabbitMQ.Client;

namespace serviceA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(value: "Hello, please enter your name : ");
            String name = Console.ReadLine();
            
            String message = $"Hello my name is, {name}";
            send(message: message);

        }
        private static void send(String message)
        {
            new ConnectionFactory().HostName = "localhost";

            using (var connection = new ConnectionFactory().CreateConnection()) 
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "myQ",durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",  routingKey: "myQ", basicProperties: null, body: body);
                    Console.WriteLine("Message sent!");
                }
            }
        }
    }
}
