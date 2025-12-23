namespace Picpay.Infra.BackgroundServices;

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Picpay.Infra.BackgroundServices.Dtos;

public class EmailConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailConsumer> _logger;
    private IConnection _connection;
    private IChannel _channel;

    public EmailConsumer(IConfiguration configuration, ILogger<EmailConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connectionString = _configuration.GetConnectionString("RabbitMqConnection");
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: "emails-enviar",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailData = JsonSerializer.Deserialize<EmailPayload>(message);

                if (emailData != null)
                {
                    await SendSmtpEmailAsync(emailData.To, emailData.Subject, emailData.Body);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail via SMTP.");
            }
        };

        await _channel.BasicConsumeAsync(queue: "emails-enviar", autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task SendSmtpEmailAsync(string to, string subject, string body)
    {
        var smtpHost = _configuration["Email:Host"];
        var smtpPort = int.Parse(_configuration["Email:Port"]);
        var senderEmail = _configuration["Email:Sender"];
        var senderPassword = _configuration["Email:Password"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage(senderEmail, to, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
        _logger.LogInformation($"E-mail enviado para {to}");
    }
}
