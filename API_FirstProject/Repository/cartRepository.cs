using API_FirstProject.Data;
using API_FirstProject.Models;

using API_FirstProject.Repository.IRepository;


namespace API_FirstProject.Repository
{
    public class cartRepository : Repository<Cart>, ICartRepository
    {
        public cartRepository(ApplicationDbContext db) :
            base(db)
        {
        }
    }
}

