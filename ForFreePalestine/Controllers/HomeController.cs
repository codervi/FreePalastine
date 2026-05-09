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

        // [DÜZELTME]: Ýki ayrý constructor yerine her iki servisi de alan TEK bir constructor býraktýk.
        public HomeController(ILogger<HomeController> logger, PageDBContext pageDB)
        {
            _logger = logger;
            _pageDb = pageDB;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult History()
        {
            // Ýleride buraya _pageDb.HistoryInfos.ToList() diyerek verileri çekeceðiz kanka ;)
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}