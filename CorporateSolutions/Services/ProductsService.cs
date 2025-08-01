using CorporateSolutions.Repositories;

namespace CorporateSolutions.Services
{
    public interface IProductService
    {
    }

    public class ProductService : IProductService
    {
        private readonly IConfiguration _config;
        private readonly IProductRepository _productRepository;

        public ProductService(IConfiguration config, IProductRepository productRepository)
        {
            _config = config;
            _productRepository = productRepository;
        }
    }

}
