using CorporateSolutions.Classes;
using CorporateSolutions.Controllers.api;
using CorporateSolutions.Models;
using CorporateSolutions.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Shouldly;
using System.Globalization;
using System.Security.Claims;

public class ProductsControllerShould
{
    private readonly IProductRepository _productRepo = Substitute.For<IProductRepository>();
    private readonly IAuditRepository _auditRepo = Substitute.For<IAuditRepository>();
    private readonly IConfiguration _config = Substitute.For<IConfiguration>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();

    private ProductsController CreateController()
    {
        var httpContext = new DefaultHttpContext();
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "User")
        }, "TestAuth");

        httpContext.User = new ClaimsPrincipal(claimsIdentity);
        _httpContextAccessor.HttpContext.Returns(httpContext);
        return new ProductsController(_productRepo, _auditRepo, _config, _httpContextAccessor);
    }

    [Theory]
    [InlineData("0.00")]
    [InlineData("0.21")]
    [InlineData("1.00")]
    [InlineData("0.075")]
    [InlineData("-0.10")]
    public async Task GetProductsShouldReturnProductsWithTotalPriceWithVat(string vatString)
    {
        _config["VAT"].Returns(vatString);
        var controller = CreateController();

        var products = new List<Product>
        {
            new Product { Id = 1, Title = "Product A", Quantity = 2, Price = 100m },
            new Product { Id = 2, Title = "Product B", Quantity = 1, Price = 50m }
        };

        _productRepo.GetProductsAsync().Returns(products);

        var result = await controller.GetProducts();
        var okResult = result.Result as OkObjectResult;
        okResult.ShouldNotBeNull();

        var productResponses = okResult.Value as List<GetProductResponse>;
        productResponses.ShouldNotBeNull();
        productResponses.Count.ShouldBe(2);

        var vat = decimal.Parse(_config["VAT"]!, CultureInfo.InvariantCulture);

        productResponses[0].TotalPriceWithVat.ShouldBe(products[0].Quantity * products[0].Price * (1 + vat));
        productResponses[1].TotalPriceWithVat.ShouldBe(products[1].Quantity * products[1].Price * (1 + vat));
    }
}
