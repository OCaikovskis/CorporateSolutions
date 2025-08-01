using CorporateSolutions.Classes;
using CorporateSolutions.Services;
using Microsoft.AspNetCore.Mvc;

namespace CorporateSolutions.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            return Ok(await _authService.LoginAsync(request));
        }
    }
}