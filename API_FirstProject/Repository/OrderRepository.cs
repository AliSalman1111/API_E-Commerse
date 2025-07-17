using API_FirstProject.Data;
using API_FirstProject.Models;

using API_FirstProject.Repository.IRepository;


namespace API_FirstProject.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
