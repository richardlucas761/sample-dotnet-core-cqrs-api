using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SampleProject.Application.Configuration.Commands;
using SampleProject.Domain.Customers;
using SampleProject.Domain.Customers.Orders;

namespace SampleProject.Application.Orders.RemoveCustomerOrder
{
    public class RemoveCustomerOrderCommandHandler(ICustomerRepository customerRepository) : ICommandHandler<RemoveCustomerOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<Unit> Handle(RemoveCustomerOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(new CustomerId(request.CustomerId));

            customer.RemoveOrder(new OrderId(request.OrderId));

            return Unit.Value;
        }

        Task IRequestHandler<RemoveCustomerOrderCommand>.Handle(RemoveCustomerOrderCommand request,
            CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}