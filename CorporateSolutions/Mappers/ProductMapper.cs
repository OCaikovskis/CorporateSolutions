using CorporateSolutions.Classes;
using CorporateSolutions.Models;

namespace CorporateSolutions.Mappers
{
    public static class ProductMapper
    {
        public static GetProductResponse ToResponse(Product product, decimal vat)
        {
            if (product == null) return null;

            return new GetProductResponse
            {
                ItemName = product.Title,
                Quantity = product.Quantity,
                Price = product.Price,
                TotalPriceWithVat  = (product.Quantity * product.Price) * (1 + vat)
            };
        }

        public static Product ToProduct(PostProductRequest product)
        {
            if (product == null) return null;

            return new Product
            {
                Title = product.Title,
                Quantity = product.Quantity,
                Price = product.Price
            };
        }
    }

}
