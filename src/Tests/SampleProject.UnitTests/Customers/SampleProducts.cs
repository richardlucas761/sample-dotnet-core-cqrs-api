using System;
using SampleProject.Domain.Products;

namespace SampleProject.UnitTests.Customers
{
    public static class SampleProducts
    {
        public static readonly ProductId Product1Id = new(Guid.NewGuid());

        public static readonly ProductId Product2Id = new(Guid.NewGuid());
    }
}