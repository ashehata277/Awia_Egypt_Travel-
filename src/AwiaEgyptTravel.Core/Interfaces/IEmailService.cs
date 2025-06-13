using AwiaEgyptTravel.Core.Entities;

namespace AwiaEgyptTravel.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmationEmailAsync(Order order, EmailTemplate template);
        Task ResendFailedEmailsAsync();
    }
}
