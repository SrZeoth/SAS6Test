using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Posts.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<String>>> GetAllPosts()
        {
            return await ConsumeMessagesAsync();
        }

        public async Task PostMessageToQueue(string message)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://lhqjdhwj:dZ-PnzUFR3LFXiIx87LRBVQ-eY2o5KFQ@goose.rmq2.cloudamqp.com/lhqjdhwj");
            string queueName = "TestQueue";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    await Task.Run(() => { // Run synchronous operation asynchronously
                        channel.BasicPublish(exchange: string.Empty,
                                             routingKey: queueName,
                                             basicProperties: null,
                                             body: body);
                    });
                }
            }

        }

        public async Task<List<string>> ConsumeMessagesAsync()
        {
            List<string> messages = new List<string>();
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://lhqjdhwj:dZ-PnzUFR3LFXiIx87LRBVQ-eY2o5KFQ@goose.rmq2.cloudamqp.com/lhqjdhwj");

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "TestQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        messages.Add(message);
                    };
                    var test = messages;
                    await Task.Run(() => channel.BasicConsume(queue: "TestQueue", autoAck: true, consumer: consumer));
                }
                return messages;
            }
            catch (Exception ex)
            {
                return messages;
            }
        }
    }
}
