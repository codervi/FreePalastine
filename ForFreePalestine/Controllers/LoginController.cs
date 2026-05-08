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

        public LoginController(PageDBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserInfo>();
        }

        // --- KAYIT OLMA (REGISTER) BÖLÜMÜ ---

        [HttpGet]
        public IActionResult Index() // Kayıt Sayfası
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
                    model.Password = _passwordHasher.HashPassword(model, model.Password);
                    _context.UserInfos.Add(model);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Kaydınız başarıyla tamamlandı! Giriş yapabilirsiniz.";
                    return RedirectToAction("LoginSide"); // Kayıttan sonra giriş sayfasına gönder
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
        public IActionResult LoginSide() // Giriş Sayfası GET
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginSide(UserInfo model)
        {
            // Giriş yaparken sadece Email ve Password kontrolü yeterli olduğu için 
            // ModelState.IsValid kontrolünü burada yapmıyoruz (çünkü modelin geri kalanı boş)

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
                        new Claim("FullName", user.UserRealName)
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