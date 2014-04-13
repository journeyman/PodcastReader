using System.Diagnostics;
using System.Linq;
using Akavache;
using Ninject;
using PodcastReader.Infrastructure.Utils.Logging;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Loaders;
using ReactiveUI;
using System.Reflection;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Infrastructure.Audio;
using Splat;

namespace PodcastReader.Phone8.Infrastructure
{
    public class AppBootstrapper : IScreen
    {
        public IRoutingState Router { get; private set; }

        public AppBootstrapper(IKernel testKernel = null, IRoutingState testRouter = null)
        {
            var kernel = testKernel ?? new StandardKernel();
            
             //Set up NInject to do DI
            var customResolver = new FuncDependencyResolver(
                (service, contract) =>
                {
                    if (contract != null) return kernel.GetAll(service, contract);
                    var items = kernel.GetAll(service);
                    var list = items.ToList();
                    return list;
                },
                (factory, service, contract) =>
                {
                    var binding = kernel.Bind(service).ToMethod(_ => factory());
                    if (contract != null) binding.Named(contract);
                });

            Locator.Current = new NInjectDependencyResolver(kernel);

            LogHost.Default.Level = LogLevel.Debug;


            //initing Router at the end to postpone call to RxApp static ctor
            this.Router = testRouter ?? new RoutingState();

            kernel.Bind<IScreen>().ToConstant(this);

            this.RegisterViews(kernel);
            this.RegisterViewModels(kernel);
            this.RegisterServices(kernel);
        
        }

        private void RegisterServices(IKernel kernel)
        {
            kernel.Rebind<IBlobCache>().ToMethod(_ => BlobCache.UserAccount).InSingletonScope();
            kernel.Rebind<ILogger>().ToMethod(_ => new PRDebugLogger()).InSingletonScope();
            kernel.Rebind<IFeedPreviewsLoader>().To<FeedPreviewsLoader>().InSingletonScope();
            kernel.Rebind<IPlayerClient>().To<BackgroundPlayerClient>().InSingletonScope();
            kernel.Rebind<ISubscriptionsManager>().To<SubscriptionsManager>().InSingletonScope();
            kernel.Rebind<ISubscriptionsCache>().To<IsoSubscriptionsCache>().InSingletonScope();
        }

        private void RegisterViewModels(IKernel kernel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var vms = assembly.GetTypes()
                .Where(t => !t.IsInterface && t.Name.EndsWith("ViewModel"));

            foreach (var viewModelType in vms)
            {
                var ifaceType = viewModelType.GetInterfaces().SingleOrDefault(iface => iface.Name.Contains(viewModelType.Name));
                if (ifaceType != null)
                    kernel.Bind(ifaceType).To(viewModelType);
            }
        }

        /// <summary>
        /// auto registers Views as IViewFo<T>
        /// </summary>
        private void RegisterViews(IKernel kernel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var views = assembly.GetTypes()
                    .Select(t => t.GetTypeAndItsRawGenericInterfaceIfExists(typeof (IViewFor<>)))
                    .Where(result => result != null);

            foreach (var implIfacePair in views)
            {
                kernel.Bind(implIfacePair.Item2).To(implIfacePair.Item1);
            }
        }
    }
}
