using System.Diagnostics;
using ForFreePalestine.Models;
using ForFreePalestine.Models.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForFreePalestine.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PageDBContext _pageDb;

        public HomeController(ILogger<HomeController> logger, PageDBContext pageDB)
        {
            _logger = logger;
            _pageDb = pageDB;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "SuperUser,Chef")]
        public IActionResult History()
        {
            // Ýleride buraya _pageDb.HistoryInfos.ToList() diyerek verileri çekeceðiz kanka ;)
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser,Chef")]
        public async Task<IActionResult> History(HistoryInfo model, IFormFile imageFile)
        {
            // 1. Validasyon barikatlarýný kaldýrýyoruz
            ModelState.Remove("Image");
            ModelState.Remove("CreatedDate");

            // 2. Tarihi manuel çakýyoruz
            model.CreatedDate = DateTime.Now;

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/history", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    model.Image = fileName;
                }

                // 3. ÝÞTE O UNUTULAN KRÝTÝK NOKTA:
                if (ModelState.IsValid)
                {
                    _pageDb.HistoryInfos.Add(model); // Önce listeye ekle
                    await _pageDb.SaveChangesAsync(); // Sonra DB'ye mühürle!

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lan hata çýktý: " + ex.Message);
            }

            // Buraya düþüyorsa bir þeyler ters gitmiþtir, verileri geri yolla kutular boþalmasýn
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}