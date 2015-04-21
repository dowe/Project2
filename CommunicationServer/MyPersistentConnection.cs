using CommunicationCommon;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer
{
    class MyPersistentConnection : PersistentConnection
    {

        private IInputCommandPipeline input = null;

        public override void Initialize(IDependencyResolver resolver)
        {
            // Dependency injection over IoC.
            input = resolver.Resolve<IInputCommandPipeline>();

            base.Initialize(resolver);
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return base.OnConnected(request, connectionId);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            input.AddReceived(data, connectionId);

            return base.OnReceived(request, connectionId, data);
        }

    }
}
