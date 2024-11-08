using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutocompleteDemo.Data;
using AutocompleteDemo.Models;

namespace AutocompleteDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return Json(new List<object>());

            var results = await _context.Items
                .Where(i => i.Name.Contains(searchTerm))
                .Take(10)
                .Select(i => new { id = i.Id, name = i.Name })
                .ToListAsync();

            return Json(results);
        }
    }
}