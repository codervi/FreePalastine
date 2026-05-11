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
        public IActionResult History()
        {
            var historyList = _pageDb.HistoryInfos.OrderBy(x => x.EventDate).ToList();
            return View(historyList);
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser,Chef")]
        public IActionResult HistoryAdd()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser,Chef")]
        public async Task<IActionResult> HistoryAdd(HistoryInfo model, IFormFile imageFile)
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

                    return RedirectToAction("History", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lan hata çýktý: " + ex.Message);
            }

            // Buraya düþüyorsa bir þeyler ters gitmiþtir, verileri geri yolla kutular boþalmasýn
            return View(model);
        }

        // --- SÝLME ÝÞLEMÝ ---
        [HttpPost]
        [Authorize(Roles = "SuperUser,Chef")]
        public async Task<IActionResult> HistoryDelete(int id)
        {
            var eventItem = await _pageDb.HistoryInfos.FindAsync(id);
            if (eventItem != null)
            {
                _pageDb.HistoryInfos.Remove(eventItem);
                await _pageDb.SaveChangesAsync();
            }
            return RedirectToAction("History");
        }

        // --- DÜZENLEME SAYFASINI AÇAN METOT (GET) ---
        [HttpGet]
        [Authorize(Roles = "SuperUser,Chef")]
        public async Task<IActionResult> HistoryEdit(int id)
        {
            // Veritabanýndan o ID'li kaydý buluyoruz
            var item = await _pageDb.HistoryInfos.FindAsync(id);

            if (item == null) return NotFound();

            return View(item); // Verileri sayfaya gönderiyoruz
        }

        // --- KAYDETME ÝÞLEMÝNÝ YAPAN METOT (POST) ---
        [HttpPost]
        [Authorize(Roles = "SuperUser,Chef")]
        public async Task<IActionResult> HistoryEdit(HistoryInfo model, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Veritabanýndaki orijinal kaydý getiriyoruz
                var existingItem = await _pageDb.HistoryInfos.FindAsync(model.HistoryId);
                if (existingItem == null) return NotFound();

                // Resim güncellenmiþse yenisini kaydet
                if (ImageFile != null)
                {
                    var extension = Path.GetExtension(ImageFile.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/history", newImageName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    existingItem.Image = newImageName;
                }

                // Diðer alanlarý güncelliyoruz
                existingItem.Title = model.Title;
                existingItem.Description = model.Description;
                existingItem.EventDate = model.EventDate;
                existingItem.Url = model.Url; // Buraya virgüllü linkleri yazýnca JS otomatik parçalayacak

                await _pageDb.SaveChangesAsync();
                return RedirectToAction("History"); // Ýþlem bitince listeye dön
            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}