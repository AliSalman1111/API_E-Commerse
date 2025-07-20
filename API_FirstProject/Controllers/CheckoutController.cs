using API_FirstProject.Models;
using API_FirstProject.Repository;
using API_FirstProject.Repository.IRepository;
using API_FirstProject.Serveses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CheckoutController : ControllerBase
    {
        private readonly IEmailSender _emailService;
        IOrderItemRepository orderItemRepository;
        IOrderRepository orderRepository;
        ICartRepository CartRepository;
        UserManager<ApplicationUser> userManager;
        public CheckoutController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, ICartRepository CartRepository, UserManager<ApplicationUser> userManager, IEmailSender _emailService)
        {
            this.orderItemRepository = orderItemRepository;
            this.orderRepository = orderRepository;
            this.CartRepository = CartRepository;
            this.userManager = userManager;
            this._emailService = _emailService;


        }

        [HttpPut("successesAsync")]
        public async Task<IActionResult> successesAsync()
        {
            var userId = userManager.GetUserId(User);


            var cartItems = CartRepository.GetAll(new Func<IQueryable<Cart>, IQueryable<Cart>>[]
            {
    q => q.Include(c => c.Product).Where(c => c.ApplicationUserId == userId)
            }).ToList();

            if (!cartItems.Any())
            {
                return NotFound();
            }


            var order = new Order
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = (decimal)cartItems.Sum(i => i.Product.price * i.count),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.count,
                    UnitPrice = (decimal)item.Product.price
                });


                item.Product.countaty -= item.count;
            }



            orderRepository.Add(order);
            orderRepository.Commit();

            string htmlContent = $@"
<h2>Thank you for your order!</h2>
<p>Order #: {order.Id}</p>
<p>Date: {order.OrderDate.ToShortDateString()}</p>
<h3>Items:</h3>
<ul>

    {string.Join("", order.OrderItems.Select(item =>
            $"<li>{item.Product.Name} x{item.Quantity} - {(item.UnitPrice * item.Quantity):C}</li>"))}
</ul>
<p><strong>Total:</strong> {(decimal)cartItems.Sum(i => i.Product.price * i.count):C}</p>";


            foreach (var item in cartItems)
            {
                CartRepository.Delete(item);
            }
            CartRepository.Commit();

            var user = await userManager.FindByIdAsync(userId);



            await _emailService.SendEmailAsync(user.Email, "Invoice for your order", htmlContent);


            return Ok();

        }
    }
}
