using InternetStore.Data;
using InternetStore.Models;
using InternetStore.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using InternetStore.Interfaces;

namespace InternetStore.Controllers
{
    public class AdminController : Controller, IAdmin
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(UserManager<User> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        public async Task<IActionResult> ManageAccounts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                var users = _userManager.Users.ToList();
                List<User> list = users.Where(u => u.Id != user.Id).ToList();
                return View(list);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> ManageProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                List<Product> list = _context.Products.ToList();
                return View(list);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> AddAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                ModelAccount account = new ModelAccount();
                return View(account);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAccount(ModelAccount account)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                if (ModelState.IsValid)
                {
                    var adduser = new User
                    {
                        Name = account.Name,
                        UserName = account.UserName,
                        Email = account.Email,
                        PhoneNumber = account.PhoneNumber,
                        Role = account.Role
                    };
                    var result = await _userManager.CreateAsync(adduser, account.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ManageAccounts");
                    }
                }
                return View(account);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> AddProduct()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                Product product = new Product();
                return View(product);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct(Product product, IFormFile Image)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    product.Image = "/images/" + uniqueFileName;
                    if (ModelState.IsValid)
                    {
                        _context.Products.Add(product);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ManageProducts");
                    }
                }

                return View(product);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditAccount(string Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                var selecteduser = await _userManager.FindByIdAsync(Id);
                if (selecteduser != null)
                {
                    ModelAccount account = new ModelAccount()
                    {
                        Id = selecteduser.Id,
                        Name = selecteduser.Name,
                        UserName = selecteduser.UserName,
                        Email = selecteduser.Email,
                        PhoneNumber = selecteduser.PhoneNumber,
                        Role = selecteduser.Role,
                        Password = selecteduser.PasswordHash
                    };
                    return View(account);
                }
                return RedirectToAction("ManageAccounts");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditAccount(ModelAccount account)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                if (ModelState.IsValid)
                {
                    var editedUser = await _userManager.FindByIdAsync(account.Id);
                    editedUser.Name = account.Name;
                    editedUser.UserName = account.UserName;
                    editedUser.Email = account.Email;
                    editedUser.PhoneNumber = account.PhoneNumber;
                    editedUser.Role = account.Role;
                    var result = await _userManager.UpdateAsync(editedUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ManageAccounts");
                    }
                }
                return View(account);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> EditProduct(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                var product = await _context.Products.FindAsync(Id);
                if (product != null)
                    return View(product);
                return RedirectToAction("ManageProducts");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProduct(Product currproduct)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                if (ModelState.IsValid)
                {
                    var film = _context.Products.FirstOrDefault(f => f.Id == currproduct.Id);
                    if (film != null)
                    {
                        film.Name = currproduct.Name;
                        film.Price = currproduct.Price;
                        _context.Products.Update(film);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ManageProducts");
                    }
                }
                return View(currproduct);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                var account = await _userManager.FindByIdAsync(id);
                if (account != null)
                {
                    var result = await _userManager.DeleteAsync(account);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ManageAccounts");
                    }
                }
                else
                {
                    return RedirectToAction("ManageAccounts");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.Role == UserRole.Admin)
            {
                var film = _context.Products.FirstOrDefault(f => f.Id == id);
                if (film != null)
                {
                    var currentDirectory = Directory.GetCurrentDirectory();
                    var imagePath = Path.Combine(currentDirectory, "wwwroot", film.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath);
                            Console.WriteLine("Файл успішно видвалений: " + imagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Помилка при видаленні файла: " + ex.Message);
                        }
                    }
                    _context.Remove(film);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("ManageProducts");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
