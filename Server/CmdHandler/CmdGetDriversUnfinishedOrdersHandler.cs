﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;

namespace Server.CmdHandler
{
    class CmdGetDriversUnfinishedOrdersHandler : CommandHandler<CmdGetDriversUnfinishedOrders>
    {

        private IServerConnection connection;

        public CmdGetDriversUnfinishedOrdersHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdGetDriversUnfinishedOrders command, string connectionIdOrNull)
        {
            List<Order> driversUnfinishedOrders = new List<Order>()
            {
                new Order() {OrderID = 0, OrderDate = DateTime.Now, Customer = new Customer() {Label = "Ultraklinik"}},
                new Order() {OrderID = 1, OrderDate = DateTime.Now, Customer = new Customer() {Label = "Lazerklink"}},
                new Order() {OrderID = 42, OrderDate = DateTime.Now, Customer = new Customer() {Label = "Dr. Awesome"}}
            };
            CmdReturnGetDriversUnfinishedOrders response = new CmdReturnGetDriversUnfinishedOrders(command.Id,
                driversUnfinishedOrders);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}