using NSubstitute;
using NUnit.Framework;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Rules;
using SampleProject.UnitTests.SeedWork;

namespace SampleProject.UnitTests.Customers
{

    [TestFixture]
    public class CustomerRegistrationTests : TestBase
    {
        private const string email = "testEmail@email.com";
        private const string Name1 = "Sample name";

        [Test]
        public void GivenCustomerEmailIsUnique_WhenCustomerIsRegistering_IsSuccessful()
        {
            // Arrange
            var customerUniquenessChecker = Substitute.For<ICustomerUniquenessChecker>();
            customerUniquenessChecker.IsUnique(email).Returns(true);

            // Act
            var customer = Customer.CreateRegistered(email, Name1, customerUniquenessChecker);

            // Assert
            AssertPublishedDomainEvent<CustomerRegisteredEvent>(customer);
        }

        [Test]
        public void GivenCustomerEmailIsNotUnique_WhenCustomerIsRegistering_BreaksCustomerEmailMustBeUniqueRule()
        {
            // Arrange
            var customerUniquenessChecker = Substitute.For<ICustomerUniquenessChecker>();
            customerUniquenessChecker.IsUnique(email).Returns(false);

            // Assert
            AssertBrokenRule<CustomerEmailMustBeUniqueRule>(() =>
            {
                // Act
                Customer.CreateRegistered(email, Name1, customerUniquenessChecker);
            });
        }
    }
}