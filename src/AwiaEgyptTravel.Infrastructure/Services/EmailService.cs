using System.Net.Mail;
using AwiaEgyptTravel.Core.Entities;
using AwiaEgyptTravel.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AwiaEgyptTravel.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;

        public EmailService(IConfiguration configuration, IOrderRepository orderRepository)
        {
            _configuration = configuration;
            _orderRepository = orderRepository;
        }

        public async Task SendOrderConfirmationEmailAsync(Order order, EmailTemplate template)
        {
            try
            {
                var body = template.Body
                    .Replace("{CustomerName}", order.CustomerName)
                    .Replace("{TourName}", order.Tour.Name)
                    .Replace("{TotalAmount}", order.TotalAmount.ToString("C"))
                    .Replace("{OrderDate}", order.CreatedAt.ToString("D"));

                using var client = CreateSmtpClient();
                var message = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:From"]),
                    Subject = template.Subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(order.Email);

                await client.SendMailAsync(message);

                order.IsEmailSent = true;
                order.EmailSentDate = DateTime.UtcNow;
                await _orderRepository.UpdateAsync(order);
            }
            catch (Exception)
            {
                // Log the error
                order.IsEmailSent = false;
                await _orderRepository.UpdateAsync(order);
                throw;
            }
        }

        public async Task ResendFailedEmailsAsync()
        {
            var pendingOrders = await _orderRepository.GetPendingEmailOrdersAsync();
            foreach (var order in pendingOrders)
            {
                var template = await _orderRepository.GetByIdAsync(order.Id);
                await SendOrderConfirmationEmailAsync(order, template);
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = _configuration["Email:SmtpHost"],
                Port = int.Parse(_configuration["Email:SmtpPort"]),
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(
                    _configuration["Email:Username"],
                    _configuration["Email:Password"])
            };
        }
    }
}
