namespace Picpay.Infra.Services;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Picpay.Domain.Entities;
using Picpay.Domain.Services;
using RabbitMQ.Client;

public class RabbitMqNotificationService : INotificationService
{
    private readonly IConfiguration _configuration;

    public RabbitMqNotificationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendNotificationAsync(User user, string message)
    {
        var connectionString = _configuration.GetConnectionString("RabbitMqConnection");
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

        // Na v7+, Connection e Channel devem ser criados com Async
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "notificacoes-transferencia",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var payload = new { email = user.Email, message = message };
        var json = JsonSerializer.Serialize(payload);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
             exchange: "",
             routingKey: "notificacoes-transferencia",
             mandatory: false,                
             basicProperties: new BasicProperties(),
             body: body);
    }
}
