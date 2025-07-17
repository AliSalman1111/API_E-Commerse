using API_FirstProject.Data;
using API_FirstProject.Models;

using API_FirstProject.Repository.IRepository;


namespace API_FirstProject.Repository
{


    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

      //  private readonly AplicationDbContext db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {


        }

    }
}
