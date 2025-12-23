namespace Picpay.Infra;

using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Picpay.Domain.Services;
using Picpay.Domain.Repositories;
using Picpay.Infra.Services;
using Picpay.Infra.Repositories;
using Picpay.Infra.BackgroundServices;


public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient("AuthorizationApi", client =>
        {
            client.BaseAddress = new Uri("https://util.devi.tools/api/v2/");
            client.Timeout = TimeSpan.FromSeconds(5); 
        });
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<INotificationService, RabbitMqNotificationService>();
        services.AddScoped<IUnitOfWorkRepository, UnitOfWork>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailService, RabbitMqEmailService>();

        services.AddHostedService<EmailConsumer>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() 
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5, 
                durationOfBreak: TimeSpan.FromSeconds(30) 
            );
    }
}
