using API_FirstProject.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeControlle : ControllerBase
    {
        private readonly IProductRepositry productRepositry;

        public HomeControlle(IProductRepositry productRepositry) {

            this.productRepositry=productRepositry;
        }

        [HttpGet]
        public IActionResult Index() {
            var products = productRepositry.GetAll();

            return Ok(products);
        }

        [HttpGet("Detales")]
        public IActionResult Detales(int Id)
        {
            var product = productRepositry.Getone(new Func<IQueryable<Models.Product>, IQueryable<Models.Product>>[]
            {

            },filter:e=>e.Id==Id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
