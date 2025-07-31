using CorporateSolutions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var message = await _context.Messages.FirstOrDefaultAsync();

            if (message == null)
            {
                return Content("No message found.");
            }

            return View(message);
        }
    }
}
