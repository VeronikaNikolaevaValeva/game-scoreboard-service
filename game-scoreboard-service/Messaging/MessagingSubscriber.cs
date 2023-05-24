using game_scoreboard_service.Messaging.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using game_scoreboard_service.Models.Messaging;
using Newtonsoft.Json.Linq;

namespace game_scoreboard_service.Messaging
{
    public class MessagingSubscriber : IMessagingSubscriber
    {
        private readonly IConnectionFactory connectionFactory;
        private NewRegisteredUser newRegisteredUser;
        private UpdatedUserScore updatedUserScore;
        public MessagingSubscriber()
        {
            connectionFactory = new ConnectionFactory()
            {
                HostName = "74.234.106.65",
                Port = 5672,
                UserName = "user",
                Password = "GPPpwQKWK@X8"
            };
        }

        public NewRegisteredUser NewRegisteredUser()
        {
            newRegisteredUser = new NewRegisteredUser();
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                IModel channel = connection.CreateModel();
                channel.ExchangeDeclare("NewRegisteredUserExchange", ExchangeType.Topic, true);

                channel.QueueDeclare("NewUserQueue", true, false, false, null);
                channel.QueueBind("NewUserQueue", "NewRegisteredUserExchange", "NewUserRoutingKey");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume("NewUserQueue", true, consumer);
            }
            return newRegisteredUser;
        }

        private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            byte[] body = e.Body.ToArray();
            string message = Encoding.Unicode.GetString(body);
            Console.WriteLine(message);
            newRegisteredUser = JsonSerializer.Deserialize<NewRegisteredUser>(message);
        }

        public UpdatedUserScore UpdateUserScore()
        {
            updatedUserScore = new UpdatedUserScore();
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                IModel channel = connection.CreateModel();
                channel.ExchangeDeclare("UpdateUserScoreExchange", ExchangeType.Topic, true);

                channel.QueueDeclare("UpdatedScoreQueue", true, false, false, null);
                channel.QueueBind("UpdatedScoreQueue", "UpdateUserScoreExchange", "UpdateScoreRoutingKey");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received_UpdateUserScore;
                channel.BasicConsume("UpdatedScoreQueue", true, consumer);
            }
            return updatedUserScore;
        }

        private void Consumer_Received_UpdateUserScore(object? sender, BasicDeliverEventArgs e)
        {
            byte[] body = e.Body.ToArray();
            string message = Encoding.Unicode.GetString(body);
            Console.WriteLine(message);
            updatedUserScore = JsonSerializer.Deserialize<UpdatedUserScore>(message);
        }

    }
}
