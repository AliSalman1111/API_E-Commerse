using API_FirstProject.Models;
using API_FirstProject.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categores = categoryRepository.GetAll();

            return Ok(categores);



        }




        [HttpPost("Create")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid) {

                categoryRepository.Add(category);
                categoryRepository.Commit();
                return Ok();
            }
            return BadRequest(category);
        }


        [HttpGet("Detalse")]
        public IActionResult Detalse(int id) {

            var category = categoryRepository.Getone(new Func<IQueryable<Category>, IQueryable<Category>>[]
            {

            },
            filter: e => e.Id == id

            );
            if (category != null) {

                return Ok(category);
            }
            return NotFound();


        }
        [HttpPut("Edit")]
        public IActionResult Edit(Category category) {

            if (ModelState.IsValid)
            {

                categoryRepository.Edit(category);
                categoryRepository.Commit();
                return Ok();
            }

            return BadRequest(category);


        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.Getone(new Func<IQueryable<Category>, IQueryable<Category>>[]
         {

         },
         filter: e => e.Id == id

         );
            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();
                return Ok();
            }
            return NotFound();

        }
        }
}
