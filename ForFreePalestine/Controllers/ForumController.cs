using ForFreePalestine.Models.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ForFreePalestine.Controllers
{
    public class ForumController : Controller
    {
        private readonly PageDBContext _context;
        public ForumController(PageDBContext pageDb)
        {
            _context = pageDb;
        }
        public IActionResult Index()
        {
            
            var threads = _context.ForumThreads
                                  .Include(t => t.User)
                                  .Include(t => t.Category)
                                  .OrderByDescending(t => t.CreatedDate) 
                                  .ToList();
            return View(threads);
        }
        [HttpGet]
        [Authorize(Roles = "SuperUser,Chef,Assistant,StandardUser")]
        public IActionResult Create()
        {
            var categories = _context.ForumCategories.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperUser,Chef,Assistant,StandardUser")]
        public IActionResult Create(ForFreePalestine.Models.ForumThread model)
        {
            ModelState.Remove("User");
            ModelState.Remove("Category");
            ModelState.Remove("Replies");

            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatedDate = DateTime.Now;
                    model.UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0");

                    _context.ForumThreads.Add(model);
                    _context.SaveChanges();

                    return RedirectToAction("Index"); 
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Konu açılırken beklenmedik bir hata oluştu. Lütfen tekrar deneyin.");

                    Console.WriteLine("HATA: " + ex.Message);
                }
            }

            var categories = _context.ForumCategories.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }
    }
}
