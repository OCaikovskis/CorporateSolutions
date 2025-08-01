using CorporateSolutions.Models;
using CorporateSolutions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CorporateSolutions.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IConfiguration _config;

        public AuditController(IAuditRepository auditRepository, IConfiguration config)
        {
            _auditRepository = auditRepository;
            _config = config;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Audit>>> GetAudit([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return Ok(await _auditRepository.GetAuditAsync(from, to));
        }
    }
}