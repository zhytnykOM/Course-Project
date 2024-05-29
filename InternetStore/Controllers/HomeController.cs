using InternetStore.Data;
using InternetStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InternetStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Result(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }
            List<Product> products = _context.Products.Where(Product => Product.Name.Contains(query)).ToList();
            return View(products);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var orderItem = _context.ModelOrders.FirstOrDefault(o => o.UserId == user.Id && o.ProductId == id);
            if (orderItem != null)
            {
                orderItem.Count++;
                _context.Update(orderItem);
            }
            else
            {
                ModelOrder modelProduct = new ModelOrder()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Count = 1,
                    UserId = user.Id
                };
                _context.ModelOrders.Add(modelProduct);
            }
            await _context.SaveChangesAsync();

            return Ok();
        }
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);
            var product = _context.ModelOrders.Where(p => p.UserId == user.Id).ToList();
            return View(product);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Cart(List<ModelOrder> orders)
        {
            var user = await _userManager.GetUserAsync(User);
            foreach (var orderItem in orders)
            {
                ModelProduct modelProduct = new ModelProduct()
                {
                    Name = orderItem.Name,
                    Price = orderItem.Price,
                    Count = orderItem.Count,
                    UserId = user.Id
                };
                _context.ModelProducts.Add(modelProduct);
            }
            var ordersToRemove = _context.ModelOrders.Where(o => o.UserId == user.Id).ToList();
            _context.ModelOrders.RemoveRange(ordersToRemove);
            await _context.SaveChangesAsync();
            return RedirectToAction("History","Account");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
