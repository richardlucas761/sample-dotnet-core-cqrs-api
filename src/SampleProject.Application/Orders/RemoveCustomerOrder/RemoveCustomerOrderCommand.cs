using System;
using SampleProject.Application.Configuration.Commands;

namespace SampleProject.Application.Orders.RemoveCustomerOrder
{
    public class RemoveCustomerOrderCommand(Guid customerId, Guid orderId) : CommandBase
    {
        public Guid CustomerId { get; } = customerId;

        public Guid OrderId { get; } = orderId;
    }
}