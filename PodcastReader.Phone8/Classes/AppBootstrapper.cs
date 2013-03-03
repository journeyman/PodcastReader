using System;
using System.Linq;
using Microsoft.Phone.Reactive;
using Ninject;
using PodcastReader.Phone8.ViewModels;
using PodcastReader.Phone8.Views;
using ReactiveUI;
using ReactiveUI.Routing;
using System.Reflection;
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

            kernel.Bind<IScreen>().ToConstant(this);

            this.RegisterViews();
            this.RegisterViewModels();

            this.Router.Navigate.Execute(RxApp.GetService<IMainViewModel>());
        }

        private IKernel GetKernel()
        {
            var kernel = new StandardKernel();


            return kernel;
        }

        private void RegisterViewModels()
        {
            var assembly = Assembly.GetExecutingAssembly();
            assembly.GetTypes()
                .Where(t => !t.IsInterface && t.Name.EndsWith("ViewModel"))
                .ToObservable()
                .Do(implType =>
                        {
                            var ifaceType = implType.GetInterfaces().SingleOrDefault(iface => iface.Name.Contains(implType.Name));
                            if (ifaceType != null)
                                RxApp.Register(implType, ifaceType);
                        })
                .Subscribe();
        }

        private void RegisterViews()
        {
            //auto registers Views as IViewFor<>
            var assembly = Assembly.GetExecutingAssembly();
            assembly.GetTypes()
                .Select(t => t.GetTypeAndItsRawGenericInterfaceIfExists(typeof(IViewFor<>)))
                .Where(result => result != null)
                .ToObservable()
                .Do(implIfacePair => RxApp.Register(implIfacePair.Item1, implIfacePair.Item2))
                .Subscribe();
        }
    }

    public static class TypeExtensions
    {
        public static Tuple<Type, Type> GetTypeAndItsRawGenericInterfaceIfExists(this Type type, Type ifaceType)
        {
            var iface = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == ifaceType);
            return iface == null ? null : new Tuple<Type, Type>(type, iface);
        }

        public static bool IsSubclassOfRawGeneric(this Type type, Type toCheck)
        {
            return null == type.GetInterfaces().SingleOrDefault(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == toCheck);
        }
    }
}
