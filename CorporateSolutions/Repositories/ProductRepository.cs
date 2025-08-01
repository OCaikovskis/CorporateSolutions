using CorporateSolutions.Exceptions;
using CorporateSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateSolutions.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductAsync(int id);
        Task<List<Product>> GetProductsAsync();
        Task<int> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product input);
        Task DeleteProductAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateProductAsync(Product input)
        {
            if (input?.Id == null)
            {
                throw new ApiException("Invalid", "Id cannot be null");
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == input.Id);
            if (product == null)
            {
                throw new ApiException("NotFound", "Product not found");
            }

            product.Title = input.Title;
            product.Quantity = input.Quantity;
            product.Price = input.Price;
            await _context.SaveChangesAsync();
        }

        public Task DeleteProductAsync(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new ApiException("NotFound", "Product not found");
            }
            _context.Products.Remove(product);
            return _context.SaveChangesAsync();
        }
    }
}