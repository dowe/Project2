using CommunicationCommon;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationClient
{
    public class ClientConnection : IClientConnection
    {

        private const int DEFAULT_RESPONSE_TIMEOUT_IN_MS = 2000;

        private Connection connection = null;

        private ReaderWriterLockSlim connectionLock = null;
        private bool pipelineRunning = false;
        private InputCommandPipeline input = null;
        private OutputCommandPipeline output = null;

        public event Action Closed = null;
        public event Action Reconnecting = null;
        public event Action Reconnected = null;

        public ClientConnection(string serverAddress)
            : this(serverAddress, new JsonCommandSerializer())
        {
        }

        public ClientConnection(string serverAddress, ICommandSerializer serializer)
        {
            connectionLock = new ReaderWriterLockSlim();

            connection = new Connection(serverAddress);
            connection.Received += connection_Received;
            connection.Reconnecting += connection_Reconnecting;
            connection.Reconnected += connection_Reconnected;
            connection.Closed += connection_Closed;

            input = new InputCommandPipeline(serializer);
            output = new OutputCommandPipeline(serializer);
        }

        public void Start()
        {
            connectionLock.EnterWriteLock();
            try
            {
                if (!pipelineRunning)
                {
                    input.Start();
                    output.Start();
                    pipelineRunning = true;
                }
            }
            finally
            {
                connectionLock.ExitWriteLock();
            }
        }

        public void Stop()
        {
            connectionLock.EnterWriteLock();
            try
            {
                if (pipelineRunning)
                {
                    output.Stop();
                    input.Stop();
                }
            }
            finally
            {
                connectionLock.ExitWriteLock();
            }
        }

        public void Connect()
        {
            connectionLock.EnterWriteLock();
            try
            {
                connection.Start().Wait();
            }
            catch (AggregateException e)
            {
                throw new ConnectionException("Failed to build up a connection to the server.", e);
            }
            finally
            {
                connectionLock.ExitWriteLock();
            }
        }

        void connection_Received(string obj)
        {
            input.AddReceived(obj, null);
        }

        void connection_Reconnected()
        {
            if (Reconnected != null)
            {
                Reconnected();
            }
        }

        void connection_Reconnecting()
        {
            if (Reconnecting != null)
            {
                Reconnecting();
            }
        }


        void connection_Closed()
        {
            if (Closed != null)
            {
                Closed();
            }
        }

        public void Send(Command command)
        {
            connectionLock.EnterWriteLock();
            try
            {
                var sendable = new Sendable<Command>(command, (str => connection.Send(str)));
                output.Send(sendable);
            }
            finally
            {
                connectionLock.ExitWriteLock();
            }
        }

        public T SendWait<T>(Command command) where T : Command
        {
            return SendWait<T>(command, DEFAULT_RESPONSE_TIMEOUT_IN_MS);
        }

        public T SendWait<T>(Command command, int timeout) where T : Command
        {
            T response = null;

            // Register a temporary handler for the expected type that receives the response.
            ResponseCommandHandler<T> responseHandler = new ResponseCommandHandler<T>(command.Id);
            input.RegisterResponseCommandHandler(responseHandler);

            Send(command);

            response = responseHandler.WaitForAnswer(timeout);

            input.DeregisterResponseCommandHandler(responseHandler);

            return response;
        }

        public void RegisterCommandHandler(ICommandHandler handler)
        {
            connectionLock.EnterWriteLock();
            try
            {
                input.RegisterCommandHandler(handler);
            }
            finally
            {
                connectionLock.ExitWriteLock();
            }
        }

    }
}
