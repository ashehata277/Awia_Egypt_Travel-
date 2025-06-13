using AwiaEgyptTravel.Core.Entities;

namespace AwiaEgyptTravel.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }

    public interface ITourRepository : IRepository<Tour>
    {
        Task<IReadOnlyList<Tour>> GetAvailableToursAsync();
        Task<Tour> GetTourWithPhotosAsync(int id);
    }

    public interface IOrderRepository : IRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetPendingEmailOrdersAsync();
    }

    public interface IEmailTemplateRepository : IRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetDefaultTemplateAsync();
    }
}
