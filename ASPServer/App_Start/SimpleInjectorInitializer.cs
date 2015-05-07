[assembly: WebActivator.PostApplicationStartMethod(typeof(ASPServer.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace ASPServer.App_Start
{
    using Common.Communication.Client;
    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Reflection;
    using System.Web.Mvc;

    public static class SimpleInjectorInitializer
    {
        public static Container container;

        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            // Did you know the container can diagnose your configuration? 
            // Go to: https://simpleinjector.org/diagnostics
            container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // Start ClientConnection and connect
            var clientConnection = ASPServer.App_Start.SimpleInjectorInitializer.container.GetInstance<IClientConnection>();
            clientConnection.Start();
            clientConnection.Connect();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            // Register the CommunicationInstance
            container.RegisterSingle<IClientConnection>(() => new ClientConnection("http://localhost:8080/commands"));
        }
    }
}
