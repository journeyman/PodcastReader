using System.Linq;
using Akavache;
using Ninject;
using ReactiveUI;
using System.Reflection;
using Pr.Core.Http;
using Pr.Core.Interfaces;
using Pr.Core.Models.Loaders;
using Pr.Core.Storage;
using Pr.Core.Utils;
using Pr.Core.Utils.Logging;
using Pr.Phone8.Infrastructure.Audio;
using Pr.Phone8.Infrastructure.Http;
using Pr.Phone8.Infrastructure.Storage;
using Pr.Phone8.Models.Loaders;
using Splat;

namespace Pr.Phone8.Infrastructure
{
    public class AppBootstrapper : IScreen
    {
        public RoutingState Router { get; private set; }

        public AppBootstrapper(IKernel testKernel = null, RoutingState testRouter = null)
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

            Locator.Current = customResolver;
            //Locator.Current = new NInjectDependencyResolver(kernel);

            LogHost.Default.Level = LogLevel.Debug;


            //initing Router at the end to postpone call to RxApp static ctor
            this.Router = testRouter ?? new RoutingState();

            kernel.Bind<IScreen>().ToConstant(this);

            this.RegisterViews(kernel);
            this.RegisterViewModels(kernel);
            this.RegisterServices(kernel);

            DebugState.Set();
        }

		private void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IBlobCache>().ToMethod(_ => BlobCache.UserAccount).InSingletonScope();
            kernel.Bind<ILogger>().ToMethod(_ => new PRDebugLogger()).InSingletonScope();
            kernel.Bind<IFeedPreviewsLoader>().To<FeedPreviewsLoader>().InSingletonScope();
            kernel.Bind<IPlayerClient>().To<BackgroundPlayerClient>().InSingletonScope();
            kernel.Bind<ISubscriptionsManager>().To<SubscriptionsManager>().InSingletonScope();
            kernel.Bind<ISubscriptionsCache>().To<IsoSubscriptionsCache>().InSingletonScope();
            kernel.Bind<IBackgroundTransferStorage>().To<BackgroundTransferStorage>().InSingletonScope();
            kernel.Bind<IBackgroundTransferConfig>().To<BackgroundTransferConfig>().InSingletonScope();
            kernel.Bind<IBackgroundDownloader>().To<BackgroundDownloader>().InSingletonScope();
            kernel.Bind<IPodcastsStorage>().To<PodcastsStorage>().InSingletonScope();
            kernel.Bind<IStorage>().To<WindowsStorage>().InSingletonScope();
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
