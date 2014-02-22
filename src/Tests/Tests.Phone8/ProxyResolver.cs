using System;
using System.Collections.Generic;
using PodcastReader.Infrastructure.Utils;
using Splat;

namespace Tests.Phone8
{
    public class ProxyResolver : IMutableDependencyResolver
    {
        private readonly IDictionary<Type, int> _servicesIndexToFilter = new Dictionary<Type, int>();
        private readonly IDictionary<Type, int> _registeredImplsCount = new Dictionary<Type, int>(); 
        
        private readonly IMutableDependencyResolver _inner;

        public ProxyResolver(IMutableDependencyResolver inner)
        {
            _inner = inner;
        }

        public void PreventOneRegistrationOf<TService>(int index = 0)
        {
            _servicesIndexToFilter[typeof (TService)] = index;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public object GetService(Type serviceType, string contract = null)
        {
            return _inner.GetService(serviceType, contract);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return _inner.GetServices(serviceType, contract);
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            int indexToFilter;
            if (_servicesIndexToFilter.TryGetValue(serviceType, out indexToFilter))
            {
                //incrementing register count
                var countRegisteredImpls = _registeredImplsCount.GetValueOrFallback(serviceType);
                _registeredImplsCount[serviceType] = countRegisteredImpls + 1;

                if (countRegisteredImpls == indexToFilter)
                    return;
            }

            _inner.Register(factory, serviceType, contract);
        }
    }
}