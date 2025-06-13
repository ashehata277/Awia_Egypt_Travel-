using AwiaEgyptTravel.Core.Entities;
using AwiaEgyptTravel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AwiaEgyptTravel.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            await UpdateAsync(entity);
        }
    }

    public class TourRepository : Repository<Tour>, ITourRepository
    {
        public TourRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Tour>> GetAvailableToursAsync()
        {
            return await _context.Tours
                .Include(t => t.Photos)
                .Where(t => t.IsAvailable && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<Tour> GetTourWithPhotosAsync(int id)
        {
            return await _context.Tours
                .Include(t => t.Photos)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }
    }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Order>> GetPendingEmailOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Tour)
                .Where(o => !o.IsEmailSent && !o.IsDeleted)
                .ToListAsync();
        }
    }

    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<EmailTemplate> GetDefaultTemplateAsync()
        {
            return await _context.EmailTemplates
                .FirstOrDefaultAsync(t => t.IsDefault && !t.IsDeleted);
        }
    }
}
