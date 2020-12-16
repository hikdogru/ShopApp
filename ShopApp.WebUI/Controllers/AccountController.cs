using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    // Tüm post metotlarında [ValidateAntiForgeryToken] yazmak yerine bunu yazabiliriz.
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {

        private UserManager<User> _userManager; // Kullanıcı girişlerini kontrol etmek için
        private SignInManager<User> _signInManager; // Cookie işlemlerini yönetmek için
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
        // Token bilgisi kontrol ediliyor.
        [ValidateAntiForgeryToken]
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
                    // Email onaylanmışsa
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Please confirm your account!");
                        return View();
                    }
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                    if (result.Succeeded)
                    {
                        // iki soru işareti null mu diye kontrol ediyor.
                        return Redirect(model.ReturnUrl ?? "~/");
                    }


                    ModelState.AddModelError("", "Email or password wrong!");

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
        // Token bilgisi kontrol ediliyor.
        [ValidateAntiForgeryToken]
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
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = code });
                await _emailSender.SendEmailAsync(model.Email, "Confirm your account", $"Please <a href='https://localhost:5001{url}'>click</a> the link for confirm the account.");
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "An unknown error occurred.Please try again.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            TempData.Put("message", new AlertMessage()
            {
                Title = "Warning",
                Message = "The session is closed.",
                        AlertType = "warning"
            });
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            if (userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Error",
                    Message = "Not valid token",
                    AlertType = "danger"
                });

                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Success",
                        Message = "Your account is confirmed.",
                        AlertType = "success"
                    });
                    return View();
                }

            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Danger",
                Message = "Your account is not confirm!",
                AlertType = "danger"
            });
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();

            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View();

            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = code });
            await _emailSender.SendEmailAsync(email, "Reset Password", $"Please <a href='https://localhost:5001{url}'>click</a> the link for reset the password.");



            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel() { Token = token };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

    }


}