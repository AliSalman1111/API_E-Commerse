using API_FirstProject.Data;
using API_FirstProject.Models;

using API_FirstProject.Repository.IRepository;


namespace API_FirstProject.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
