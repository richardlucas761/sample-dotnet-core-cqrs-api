using SampleProject.Domain.Products;
using SampleProject.Domain.SharedKernel;

namespace SampleProject.UnitTests.Customers
{
    public static class SampleProductPrices
    {
        public static readonly ProductPriceData Product1EUR = new(
            SampleProducts.Product1Id,
            MoneyValue.Of(100, "EUR"));

        public static readonly ProductPriceData Product1USD = new(
            SampleProducts.Product1Id,
            MoneyValue.Of(110, "USD"));
    }
}