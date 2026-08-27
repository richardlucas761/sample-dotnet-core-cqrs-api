using System;
using System.Collections.Generic;
using NUnit.Framework;
using SampleProject.Domain.Customers.Orders;
using SampleProject.Domain.Customers.Orders.Events;
using SampleProject.Domain.Customers.Rules;
using SampleProject.Domain.ForeignExchange;
using SampleProject.Domain.Products;
using SampleProject.Domain.SharedKernel;
using SampleProject.UnitTests.SeedWork;

namespace SampleProject.UnitTests.Customers
{
    [TestFixture]
    public class PlaceOrderTests : TestBase
    {
        [Test]
        public void PlaceOrder_WhenAtLeastOneProductIsAdded_IsSuccessful()
        {
            // Arrange
            var customer = CustomerFactory.Create();

            var orderProductsData = new List<OrderProductData>
            {
                new(SampleProducts.Product1Id, 2)
            };

            var allProductPrices = new List<ProductPriceData>
            {
                SampleProductPrices.Product1EUR, SampleProductPrices.Product1USD
            };

            var conversionRates = GetConversionRates();

            // Act
            customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                Constants.currencyEuro,
                conversionRates);

            // Assert
            var orderPlaced = AssertPublishedDomainEvent<OrderPlacedEvent>(customer);
            Assert.That(orderPlaced.Value, Is.EqualTo(MoneyValue.Of(200, Constants.currencyEuro)));
        }

        [Test]
        public void PlaceOrder_WhenNoProductIsAdded_BreaksOrderMustHaveAtLeastOneProductRule()
        {
            // Arrange
            var customer = CustomerFactory.Create();

            var orderProductsData = new List<OrderProductData>();

            var allProductPrices = new List<ProductPriceData>
            {
                SampleProductPrices.Product1EUR, SampleProductPrices.Product1USD
            };

            var conversionRates = GetConversionRates();

            // Assert
            AssertBrokenRule<OrderMustHaveAtLeastOneProductRule>(() =>
            {
                // Act
                customer.PlaceOrder(
                    orderProductsData,
                    allProductPrices,
                    Constants.currencyEuro,
                    conversionRates);
            });
        }

        [Test]
        public void PlaceOrder_GivenTwoOrdersInThatDayAlreadyMade_BreaksCustomerCannotOrderMoreThan2OrdersOnTheSameDayRule()
        {
            // Arrange
            var customer = CustomerFactory.Create();

            var orderProductsData = new List<OrderProductData>
            {
                new OrderProductData(SampleProducts.Product1Id, 2)
            };

            var allProductPrices = new List<ProductPriceData>
            {
                SampleProductPrices.Product1EUR, SampleProductPrices.Product1USD
            };

            var conversionRates = GetConversionRates();

            // TODO is this setting the actual system clock? Seems like a really bad idea.

            SystemClock.Set(new DateTime(2020, 1, 10, 11, 0, 0));
            customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                Constants.currencyEuro,
                conversionRates);

            SystemClock.Set(new DateTime(2020, 1, 10, 11, 30, 0));
            customer.PlaceOrder(
                orderProductsData,
                allProductPrices,
                Constants.currencyEuro,
                conversionRates);

            SystemClock.Set(new DateTime(2020, 1, 10, 12, 00, 0));

            // Assert
            AssertBrokenRule<CustomerCannotOrderMoreThan2OrdersOnTheSameDayRule>(() =>
            {
                // Act
                customer.PlaceOrder(
                    orderProductsData,
                    allProductPrices,
                    Constants.currencyEuro,
                    conversionRates);
            });
        }

        private static List<ConversionRate> GetConversionRates()
        {
            var conversionRates = new List<ConversionRate>
            {
                new(Constants.currencyUsDollar, Constants.currencyEuro, Constants.UsEuro),
                new(Constants.currencyEuro, Constants.currencyUsDollar, Constants.EuroUs)
            };

            return conversionRates;
        }
    }
}