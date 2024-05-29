using InternetStore.Data;
using InternetStore.Interfaces;
using InternetStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace InternetStore.Controllers
{
    public class AccountController : Controller, IAccount
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        public delegate Task UserEventHandler(string userId, string message);
        public event UserEventHandler UserEventOccurred;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            UserEventOccurred += async (userId, message) =>
            {
                TempData["UserNotification"] = message;
            };
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ModelLogin model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    await UserEventOccurred?.Invoke(user.Id, "Успішна авторизація!");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Помилка авторизації!");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ModelRegister model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Name = model.Name, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await UserEventOccurred?.Invoke(user.Id, "Користувач успішно зареєстрований!");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Помилка реєстрації!");
                return View(model);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ModelProfile
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ModelProfile model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await UserEventOccurred?.Invoke(user.Id, "Дані успішно зміненні!");
                    return View(model);
                }
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            ModelChangePassword model = new ModelChangePassword();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ModelChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction(nameof(Login));
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                await _signInManager.RefreshSignInAsync(user);
                await UserEventOccurred?.Invoke(user.Id, "Успішна зміна пароля!");
                return View(model);
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            List<ModelProduct> products = _context.ModelProducts.Where(p => p.UserId == user.Id).ToList();
            return View(products);
        }
        
    }
}
