using CorporateSolutions.Classes;
using CorporateSolutions.Exceptions;
using CorporateSolutions.Mappers;
using CorporateSolutions.Models;
using CorporateSolutions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace CorporateSolutions.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly decimal _vat;

        public ProductsController(
            IProductRepository productRepository,
            IAuditRepository auditRepository,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _auditRepository = auditRepository;
            _config = config;
            _vat = decimal.Parse(config["VAT"]!, CultureInfo.InvariantCulture);
            _httpContextAccessor=httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetProductResponse>>> GetProducts()
        {
            var product = await _productRepository.GetProductsAsync();
            var productResponse = new List<GetProductResponse>();
            foreach (var item in product)
            {
                var response = ProductMapper.ToResponse(item, _vat);
                if (response != null)
                {
                    productResponse.Add(response);
                }
            }
            return Ok(productResponse);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            var productResponse = ProductMapper.ToResponse(product, _vat);
            return Ok(productResponse);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] PostProductRequest input)
        {
            var product = ProductMapper.ToProduct(input);
            var id = await _productRepository.CreateProductAsync(product);
            product.Id = id;
            var changes = $"Created: {System.Text.Json.JsonSerializer.Serialize(product)}";

            await LogAuditAsync(id, changes);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product input)
        {
            var existing = await _productRepository.GetProductAsync(input.Id);
            if (existing == null)
            {
                throw new ApiException("NotFound", "Product not found");
            }

            await _productRepository.UpdateProductAsync(input);

            var changes = $"Old: {System.Text.Json.JsonSerializer.Serialize(existing)}\n" +
                  $"New: {System.Text.Json.JsonSerializer.Serialize(input)}";

            await LogAuditAsync(input.Id, changes);
            return Ok(input);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existing = await _productRepository.GetProductAsync(id);
            if (existing == null)
            {
                throw new ApiException("NotFound", "Product not found");
            }
            await _productRepository.DeleteProductAsync(id);
            var changes = $"Deleted: {System.Text.Json.JsonSerializer.Serialize(existing)}";

            await LogAuditAsync(id, changes);
            return Ok(new { message = $"Deleted product {id}" });
        }

        private async Task LogAuditAsync(int productId, string changeDescription)
        {
            var username = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Unknown";
            var audit = new Audit
            {
                Action = changeDescription,
                Changes = $"Product ID: {productId}",
                Timestamp = DateTime.UtcNow,
                Username = username
            };

            await _auditRepository.LogAuditAsync(audit);
        }
    }
}