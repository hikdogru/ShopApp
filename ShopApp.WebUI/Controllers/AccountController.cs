using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<User> _userManager; // Kullanıcı girişlerini kontrol etmek için
        private SignInManager<User> _signInManager; // Cookie işlemlerini yönetmek için
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {

            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                // var user = await _userManager.FindByNameAsync(model.UserName);
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Login failed!");
                    return View(model);
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                    if (result.Succeeded)
                    {
                        // iki soru işareti null mu diye kontrol ediyor.
                        return Redirect(model.ReturnUrl ?? "~/");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Email or password wrong!");
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
            };


            var result = await _userManager.CreateAsync(user, model.Password); // Kullanıcı oluşturma işlemi
            // Kullanıcı oluşturma başarılı ise
            if (result.Succeeded)
            {
                //generate token
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "An unknown error occurred.Please try again.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
    }


}