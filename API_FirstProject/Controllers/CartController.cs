using API_FirstProject.Models;
using API_FirstProject.Repository;
using API_FirstProject.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Stripe;
namespace API_FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartRepository cartRepository,UserManager<ApplicationUser> userManager) {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }
        [HttpPost("AddToCart")]
        public ActionResult AddToCart(int count,int pId) {

            var user = userManager.GetUserId(User);

            var product = cartRepository.Getone(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
        {


        }, filter: e => e.ApplicationUserId == user && e.ProductId == pId


        );
            if (user != null) {
            
                Cart cart = new Cart()
                {
                    ApplicationUserId= user,
                    ProductId= pId,
                    count= count
                };

                if (product != null) {

                    product.count += count;
                }
                else
                {
                    cartRepository.Add(cart);

                }
                cartRepository.Commit();

                return Ok(cart);    
            }
               return NotFound();
        
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var userid = userManager.GetUserId(User);



            var products = cartRepository.GetAll(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
            {
    q=>q.Include(x=>x.Product).Where(e=>e.ApplicationUserId == userid)
            });

          //  ViewBag.products = products.Sum(x => x.Product.price * x.count);
            return Ok(products);

        }
        [HttpPut("Increment")]
        public IActionResult Increment(int Id)
        {

            var userid = userManager.GetUserId(User);
            var product = cartRepository.Getone(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
            {
             
                
            }, filter: e => e.ApplicationUserId == userid && e.ProductId == Id


            );

            if (product != null)
            {
                product.count++;
                cartRepository.Commit();
               return Ok(product);

            }

            return NotFound();
        }


        [HttpPut("Decrement")]

        public IActionResult Decrement(int Id)
        {

            var userid = userManager.GetUserId(User);
            var product = cartRepository.Getone(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
            {

            }, filter: e => e.ApplicationUserId == userid && e.ProductId == Id


            );
            if (product != null)
            {
                product.count--;

                if (product.count > 0)

                    cartRepository.Commit();
                else
                    product.count = 1;

                return Ok(product); 

            }

            return NotFound();
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(int Id)
        {

            var userid = userManager.GetUserId(User);
            var product = cartRepository.Getone(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
            {

            }, filter: e => e.ApplicationUserId == userid && e.ProductId == Id


            );
            if (product != null)
            {
                cartRepository.Delete(product);
                cartRepository.Commit();
                return Ok();

            }

            return NotFound();
        }

        [HttpPost("Pay")]
        public IActionResult Pay()
        {

            var userid = userManager.GetUserId(User);



            var products = cartRepository.GetAll(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
            {
    q=>q.Include(x=>x.Product).Where(e=>e.ApplicationUserId == userid)
            });

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in products.ToList())
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = item.Product.Description,
                        },
                        UnitAmount = (long)item.Product.price * 100,
                    },
                    Quantity = item.count,

                });
            }
            var service = new SessionService();
            var session = service.Create(options);
            return Ok(session.Url);
        }


    }
}
