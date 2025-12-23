namespace Picpay.Infra;

using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; 
using System.Text;
using System.Text.Json;
using Picpay.Domain.Services;
using RabbitMQ.Client;

public class RabbitMqEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public RabbitMqEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string body)
    {
        var connectionString = _configuration.GetConnectionString("RabbitMqConnection");
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "emails-enviar",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var payload = new 
        { 
            To = email, 
            Subject = subject, 
            Body = body 
        };
        
        var json = JsonSerializer.Serialize(payload);
        var messageBody = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
             exchange: "",
             routingKey: "emails-enviar",
             mandatory: false,
             basicProperties: new BasicProperties(),
             body: messageBody);
    }
}
