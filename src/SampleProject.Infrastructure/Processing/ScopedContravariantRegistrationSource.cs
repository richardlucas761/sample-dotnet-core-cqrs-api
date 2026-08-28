using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac.Features.Variance;

namespace SampleProject.Infrastructure.Processing
{
    public partial class MediatorModule
    {
        private class ScopedContravariantRegistrationSource : IRegistrationSource
        {
            private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
            private readonly List<Type> _types = new List<Type>();

            public ScopedContravariantRegistrationSource(params Type[] types)
            {
                if (types == null)
                    throw new ArgumentNullException(nameof(types));
                if (!types.All(x => x.IsGenericTypeDefinition))
                    throw new ArgumentException("Supplied types should be generic type definitions");
                _types.AddRange(types);
            }

            public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
            {
                var components = _source.RegistrationsFor(service, registrationAccessor);
                foreach (var c in components)
                {
                    var defs = c.Target.Services
                        .OfType<TypedService>()
                        .Select(x => x.ServiceType.GetGenericTypeDefinition());

                    if (defs.Any(_types.Contains))
                        yield return c;
                }
            }

            public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
            {
                throw new NotImplementedException();
            }

            public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
        }
    }
}