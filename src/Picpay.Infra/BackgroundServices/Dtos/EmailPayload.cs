namespace Picpay.Infra.BackgroundServices.Dtos;

public record class EmailPayload(string To, string Subject, string Body)
{

}
