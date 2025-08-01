namespace CorporateSolutions.Classes
{
    public class GetProductResponse
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPriceWithVat { get; set; }

    }
}
