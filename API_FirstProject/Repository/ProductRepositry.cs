using API_FirstProject.Data;
using API_FirstProject.Models;

using API_FirstProject.Repository.IRepository;


namespace API_FirstProject.Repository
{
    public class ProductRepositry : Repository<Product>, IProductRepositry
    {
        public ProductRepositry(ApplicationDbContext db) : base(db)
        {

        }
    }
}
