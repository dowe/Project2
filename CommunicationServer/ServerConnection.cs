﻿using CommunicationCommon;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Host.HttpListener;

namespace CommunicationServer
{
    public class ServerConnection : CommunicationServer.IServerConnection
    {

        private string bindAddress = null;

        private InputCommandPipeline input = null;
        private OutputCommandPipeline output = null;

        public ServerConnection()
            : this("http://localhost:8080")
        {
        }

        public ServerConnection(string bindAddress)
            : this(bindAddress, new JsonCommandSerializer())
        {
        }

        public ServerConnection(string bindAddress, ICommandSerializer serializer)
        {
            this.bindAddress = bindAddress;

            this.input = new InputCommandPipeline(serializer);
            this.output = new OutputCommandPipeline(serializer);
        }

        public void RunForever()
        {
            input.Start();
            output.Start();
            // IoC Dependency Injection for the connection.
            GlobalHost.DependencyResolver.Register(typeof(IInputCommandPipeline), () => input);
            try
            {
                using (WebApp.Start(bindAddress))
                {
                    Console.WriteLine("Server running on {0}", bindAddress);
                    while (true) ;
                }
            }
            finally
            {
                output.Stop();
                input.Stop();
            }
        }

        private IPersistentConnectionContext GetConnectionContext()
        {
            return GlobalHost.ConnectionManager.GetConnectionContext<MyPersistentConnection>();
        }

        public void Unicast(Command command, string connectionId)
        {
            IPersistentConnectionContext context = GetConnectionContext();
            Sendable<Command> sendable = new Sendable<Command>(command, (str => context.Connection.Send(connectionId, str)));
            output.Send(sendable);
        }

        public void Multicast(Command command, string groupId)
        {
            IPersistentConnectionContext context = GetConnectionContext();
            Sendable<Command> sendable = new Sendable<Command>(command, (str => context.Groups.Send(str, groupId)));
            output.Send(sendable);
        }

        public void Broadcast(Command command)
        {
            IPersistentConnectionContext context = GetConnectionContext();
            Sendable<Command> sendable = new Sendable<Command>(command, (str => context.Connection.Broadcast(str)));
            output.Send(sendable);
        }

        public void RegisterCommandHandler(ICommandHandler handler)
        {
            input.RegisterCommandHandler(handler);
        }

        public string BindAddress
        {
            get
            {
                return bindAddress;
            }
        }


    }
}
