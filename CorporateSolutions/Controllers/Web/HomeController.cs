using CorporateSolutions.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CorporateSolutions.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public HomeController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var message = await _messageRepository.GetFirstMessageAsync();
            if (message == null)
            {
                return Content("No message found.");
            }

            return View(message);
        }
    }
}
