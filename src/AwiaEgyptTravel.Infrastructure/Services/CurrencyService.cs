namespace AwiaEgyptTravel.Infrastructure.Services
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency);
        bool IsValidCurrency(string currency);
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly Dictionary<string, decimal> _exchangeRates;

        public CurrencyService(ILogger<CurrencyService> logger)
        {
            _logger = logger;
            // Mock exchange rates (in production, these would come from an API)
            _exchangeRates = new Dictionary<string, decimal>
            {
                { "USD", 1m },
                { "EGP", 30.90m } // Example rate
            };
        }

        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            if (!IsValidCurrency(fromCurrency) || !IsValidCurrency(toCurrency))
            {
                throw new ArgumentException("Invalid currency code");
            }

            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            // Convert to USD first if not USD
            var usdAmount = fromCurrency == "USD" 
                ? amount 
                : amount / _exchangeRates[fromCurrency];

            // Convert from USD to target currency
            var result = toCurrency == "USD" 
                ? usdAmount 
                : usdAmount * _exchangeRates[toCurrency];

            return Math.Round(result, 2);
        }

        public bool IsValidCurrency(string currency)
        {
            return _exchangeRates.ContainsKey(currency.ToUpperInvariant());
        }
    }
}
