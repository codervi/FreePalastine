using ForFreePalestine.Models;
using ForFreePalestine.Models.DataContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ForFreePalestine.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly PageDBContext _context;
        private readonly PasswordHasher<UserInfo> _passwordHasher;
        private readonly IConfiguration _configuration;

        public LoginController(PageDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<UserInfo>();
        }

        // --- KAYIT OLMA (REGISTER) BÖLÜMÜ ---

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserInfo model)
        {
            bool isEmailExist = _context.UserInfos.Any(x => x.Email == model.Email);
            bool isUserNameExist = _context.UserInfos.Any(x => x.UserName == model.UserName);

            if (isEmailExist) ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanımda.");
            if (isUserNameExist) ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten alınmış.");

            if (ModelState.IsValid)
            {
                try
                {
                    // [NOT]: Appsettings'den admin mailini okuyup rütbe ataması yapıyoruz.
                    string adminEmail = _configuration["AdminSettings:AdminEmail"];
                    if (model.Email.Trim().ToLower() == adminEmail.Trim().ToLower())
                    {
                        model.Role = UserRoles.SuperUser;
                    }
                    else
                    {
                        model.Role = UserRoles.StandardUser;
                    }

                    model.Password = _passwordHasher.HashPassword(model, model.Password);
                    _context.UserInfos.Add(model);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Kaydınız başarıyla tamamlandı! Giriş yapabilirsiniz.";
                    return RedirectToAction("LoginSide");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Kayıt hatası: " + ex.Message);
                }
            }
            return View(model);
        }

        // --- GİRİŞ YAPMA (LOGIN) BÖLÜMÜ ---

        [HttpGet]
        public IActionResult LoginSide()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginSide(UserInfo model)
        {
            var user = _context.UserInfos.FirstOrDefault(u => u.Email == model.Email);

            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("FullName", user.UserRealName),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Incorrect password!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}