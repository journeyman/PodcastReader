using System;
using System.Collections.Generic;
using Ninject;
using Splat;

namespace PodcastReader.Phone8.Infrastructure
{
    public class NInjectDependencyResolver : IMutableDependencyResolver, IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NInjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }

        public object GetService(Type serviceType, string contract = null)
        {
            if (contract != null) return _kernel.Get(serviceType, contract);
            return _kernel.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            if (contract != null) return _kernel.GetAll(serviceType, contract);
            return _kernel.GetAll(serviceType);
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            var binding = _kernel.Rebind(serviceType).ToMethod(_ => factory());
            if (contract != null) binding.Named(contract);
        }
    }
}