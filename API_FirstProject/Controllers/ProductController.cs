using API_FirstProject.DTOs;
using API_FirstProject.Models;
using API_FirstProject.Repository;
using API_FirstProject.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositry productRepositry;

        public ProductController(IProductRepositry productRepositry)
        {
            this.productRepositry = productRepositry;
        }
        [HttpGet]
        public ActionResult Index(int page, string? search = null) {

            if (page <= 0)

                page = 1;
            var products = productRepositry.GetAll(new Func<IQueryable<Models.Product>, IQueryable<Models.Product>>[]
            {
                e=>e.Include(e=>e.Category)
            });

            if (search != null) {
                search = search.TrimStart();
                search = search.TrimEnd();
                products = products.Where(e => e.Name.Contains(search));

            }

            products = products.Skip((page - 1) * 5).Take(5);


            if (products.Any())
            {
                return Ok(products.ToList());
            }

            return NoContent();
        }


        [HttpPost("Create")]
        public IActionResult Create(ProductDTO product)
        {
            if (ModelState.IsValid)
            {


                if (product.photo.Length > 0)
                {
                    // var filePath = Path.GetTempFileName();

                    var fileName = Guid.NewGuid() + Path.GetExtension(product.photo.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        product.photo.CopyTo(stream);
                    }


                }
                // Category category=new Category() { Name= CategoryName };

                var Product = new Product()
                {

                    Name = product.Name,
                    Description = product.Description,
                    photo = product.photo.FileName,
                    price = product.price,
                    categoryId = product.categoryId,
                    companyId = product.companyId,
                    countaty = product.countaty,


                };
                productRepositry.Add(Product);
                productRepositry.Commit();

                return Ok(Product);
            }


            // Product product = new Product();
            return BadRequest(product);
        }

        [HttpGet("detales")]
        public IActionResult detales(int Id)
        {
            var product = productRepositry.Getone(new Func<IQueryable<Product>, IQueryable<Product>>[]
{
                       q => q.Include(c => c.Category)


},
filter: c => c.Id == Id

);
           
           
            return Ok(product);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(ProductDTO product)
        {
            var oldproduct = productRepositry.Getone(new Func<IQueryable<Product>, IQueryable<Product>>[]
{
                       q => q.Include(c => c.Category)


},
       filter: c => c.Id == product.Id


);
            // product.photo = oldproduct.photo;

            // if (ModelState.IsValid)
            // {


            if (product.photo != null && product.photo.Length > 0)
            {
                // var filePath = Path.GetTempFileName();

                var fileName = Guid.NewGuid() + Path.GetExtension(product.photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", oldproduct.photo);


                using (var stream = System.IO.File.Create(filePath))
                {
                    product.photo.CopyTo(stream);
                }

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);

                }
                oldproduct.photo = fileName;
            }
            else
            {

                oldproduct.photo = oldproduct.photo;
            }
            // Category category=new Category() { Name= CategoryName };
            productRepositry.Edit(oldproduct);
            productRepositry.Commit();
          
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(1);

            Response.Cookies.Append("succes", "Edit Product Succesfuly", cookieOptions);
            return Ok(oldproduct);

        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int Id)
        {
            var product = productRepositry.Getone(new Func<IQueryable<Product>, IQueryable<Product>>[]
{
                       q => q.Include(c => c.Category)


},
filter: c => c.Id == Id

);
            // Category category=new Category() { Name= CategoryName };
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.photo);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);

            }

            productRepositry.Delete(product);
            productRepositry.Commit();
          
            return Ok(product);
        }

    }
}
