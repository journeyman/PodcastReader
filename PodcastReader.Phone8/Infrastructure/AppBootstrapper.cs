﻿using System;
using System.Linq;
using Ninject;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Loaders;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using System.Reflection;
using PodcastReader.Infrastructure.Interfaces;
using PodcastReader.Infrastructure.Audio;

namespace PodcastReader.Phone8.Infrastructure
{
    public class AppBootstrapper : IScreen
    {
        public IRoutingState Router { get; private set; }

        public AppBootstrapper(IKernel testKernel = null, IRoutingState testRouter = null)
        {
            var kernel = testKernel ?? this.GetKernel();
            this.Router = testRouter ?? new RoutingState();
            
            // Set up NInject to do DI
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
            customResolver.InitializeResolver();

            RxApp.DependencyResolver = customResolver;

            LogHost.Default.Level = LogLevel.Debug;

            kernel.Bind<IScreen>().ToConstant(this);

            this.RegisterViews(kernel);
            this.RegisterViewModels(kernel);
            this.RegisterServices(kernel);

            this.Router.Navigate.Execute(RxApp.DependencyResolver.GetService<IMainViewModel>());
        }

        private IKernel GetKernel()
        {
            return new StandardKernel();
        }

        private void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IFeedPreviewsLoader>().To<FeedPreviewsLoader>().InSingletonScope();
            kernel.Bind<IPlayerClient>().To<BufferingPlayerClient>().InSingletonScope();
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

            foreach (var ifaceImplPair in views)
            {
                kernel.Bind(ifaceImplPair.Item1).To(ifaceImplPair.Item2);
            }
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