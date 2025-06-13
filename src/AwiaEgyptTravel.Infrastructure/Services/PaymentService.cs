using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace AwiaEgyptTravel.Infrastructure.Services
{
    public interface IPaymentService
    {
        Task<(bool success, string transactionId)> ProcessPaymentAsync(string cardNumber, string expiryMonth, string expiryYear, string cvc, decimal amount, string currency);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly string _stripeSecretKey;

        public PaymentService(ILogger<PaymentService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _stripeSecretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public async Task<(bool success, string transactionId)> ProcessPaymentAsync(string cardNumber, string expiryMonth, string expiryYear, string cvc, decimal amount, string currency)
        {            try
            {
                var tokenOptions = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = cardNumber,
                        ExpMonth = long.Parse(expiryMonth),
                        ExpYear = long.Parse(expiryYear),
                        Cvc = cvc
                    }
                };

                var tokenService = new TokenService();
                Token stripeToken = await tokenService.CreateAsync(tokenOptions);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Stripe expects amount in cents
                    Currency = currency.ToLower(),
                    PaymentMethod = stripeToken.Card.Id,
                    Confirm = true,
                    Description = "Tour booking payment"
                };

                var service = new PaymentIntentService();
                var payment = await service.CreateAsync(options);

                if (payment.Status == "succeeded")
                {
                    _logger.LogInformation($"Payment processed successfully. TransactionId: {payment.Id}");
                    return (true, payment.Id);
                }

                _logger.LogWarning($"Payment failed. Status: {payment.Status}");
                return (false, null);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error processing payment: {Message}", ex.Message);
                return (false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return (false, null);
            }
        }
    }
}
