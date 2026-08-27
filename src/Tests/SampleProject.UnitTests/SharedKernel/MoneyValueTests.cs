using System.Collections.Generic;
using NUnit.Framework;
using SampleProject.Domain.SharedKernel;
using SampleProject.UnitTests.SeedWork;

namespace SampleProject.UnitTests.SharedKernel
{
    [TestFixture]
    public class MoneyValueTests : TestBase
    {
        [Test]
        public void MoneyValueOf_WhenCurrencyIsProvided_IsSuccessful()
        {
            var value = MoneyValue.Of(120, Constants.currencyEuro);

            Assert.That(value.Value, Is.EqualTo(120));
            Assert.That(value.Currency, Is.EqualTo(Constants.currencyEuro));
        }

        [Test]
        public void MoneyValueOf_WhenCurrencyIsNotProvided_ThrowsMoneyValueMustHaveCurrencyRuleBroken()
        {
            AssertBrokenRule<MoneyValueMustHaveCurrencyRule>(() =>
            {
                MoneyValue.Of(120, "");
            });
        }

        [Test]
        public void GivenTwoMoneyValuesWithTheSameCurrencies_WhenAddThem_IsSuccessful()
        {
            var valueInEuros = MoneyValue.Of(100, Constants.currencyEuro);
            var valueInEuros2 = MoneyValue.Of(50, Constants.currencyEuro);

            MoneyValue add = valueInEuros + valueInEuros2;

            Assert.That(add.Value, Is.EqualTo(150));
            Assert.That(add.Currency, Is.EqualTo(Constants.currencyEuro));
        }

        [Test]
        public void GivenTwoMoneyValuesWithTheSameCurrencies_SumThem_IsSuccessful()
        {
            var valueInEuros = MoneyValue.Of(100, Constants.currencyEuro);
            var valueInEuros2 = MoneyValue.Of(50, Constants.currencyEuro);

            IList<MoneyValue> values = new List<MoneyValue>
            {
                valueInEuros, valueInEuros2
            };

            MoneyValue add = values.Sum();

            Assert.That(add.Value, Is.EqualTo(150));
            Assert.That(add.Currency, Is.EqualTo(Constants.currencyEuro));
        }

        [Test]
        public void GivenTwoMoneyValuesWithDifferentCurrencies_WhenAddThem_ThrowsMoneyValueOperationMustBePerformedOnTheSameCurrencyRule()
        {
            var valueInEuros = MoneyValue.Of(100, Constants.currencyEuro);
            var valueInDollars = MoneyValue.Of(50, Constants.currencyUsDollar);
            AssertBrokenRule<MoneyValueOperationMustBePerformedOnTheSameCurrencyRule>(() =>
            {
                var add = valueInEuros + valueInDollars;
            });
        }
    }
}