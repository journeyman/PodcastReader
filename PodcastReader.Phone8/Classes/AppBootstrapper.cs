using Ninject;
using PodcastReader.Phone8.ViewModels;
using PodcastReader.Phone8.Views;
using ReactiveUI;
using ReactiveUI.Routing;
namespace PodcastReader.Phone8.Classes
{
    public class AppBootstrapper : IScreen
    {
        public IRoutingState Router { get; private set; }

        public AppBootstrapper(IKernel testKernel = null, IRoutingState testRouter = null)
        {
            var kernel = testKernel ?? this.GetKernel();
            this.Router = testRouter ?? new RoutingState();
            
            // Set up NInject to do DI
            RxApp.ConfigureServiceLocator(
                (iface, contract) =>
                {
                    if (contract != null) return kernel.Get(iface, contract);
                    return kernel.Get(iface);
                },
                (iface, contract) =>
                {
                    if (contract != null) return kernel.GetAll(iface, contract);
                    return kernel.GetAll(iface);
                },
                (realClass, iface, contract) =>
                {
                    var binding = kernel.Bind(iface).To(realClass);
                    if (contract != null) binding.Named(contract);
                });

            LogHost.Default.Level = LogLevel.Debug;

            this.Router.Navigate.Execute(RxApp.GetService<IMainViewViewModel>());
        }

        private IKernel GetKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IScreen>().ToConstant(this);

            //ViewModels
            kernel.Bind<IMainViewViewModel>().To<MainViewViewModel>().InSingletonScope();

            kernel.Bind<IViewFor<IMainViewViewModel>>().To<MainView>();

            return kernel;
        }
    }
}
