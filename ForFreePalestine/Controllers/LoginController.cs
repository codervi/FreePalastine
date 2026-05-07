using ForFreePalestine.Models;
using ForFreePalestine.Models.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ForFreePalestine.Controllers
{
    public class LoginController : Controller
    {
        private readonly PageDBContext _context;
        private readonly PasswordHasher<UserInfo> _passwordHasher;

        public LoginController(PageDBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserInfo>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserInfo model)
        {
            // 1. ADIM: E-posta ve Kullanıcı Adı Kontrolü (Unique Check)
            // Veritabanında bu mail veya kullanıcı adı zaten var mı?
            bool isEmailExist = _context.UserInfos.Any(x => x.Email == model.Email);
            bool isUserNameExist = _context.UserInfos.Any(x => x.UserName == model.UserName);

            if (isEmailExist)
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanımda.");
            }

            if (isUserNameExist)
            {
                ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten alınmış.");
            }

            // 2. ADIM: Genel Validasyon Kontrolü
            if (ModelState.IsValid)
            {
                try
                {
                    // 3. ADIM: Şifreyi Hash'leme
                    model.Password = _passwordHasher.HashPassword(model, model.Password);

                    // 4. ADIM: Kayıt İşlemi
                    _context.UserInfos.Add(model);
                    _context.SaveChanges();

                    // Kayıt başarılıysa bir mesaj gönderip yönlendirelim
                    TempData["SuccessMessage"] = "Kaydınız başarıyla tamamlandı!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Beklenmedik bir veritabanı hatası olursa (Loglama yapılabilir)
                    ModelState.AddModelError("", "Kayıt sırasında teknik bir hata oluştu: " + ex.Message);
                }
            }

            // Eğer bir hata varsa (mail varlığı veya eksik bilgi), aynı sayfaya hatalarla döner
            return View(model);
        }
    }
}