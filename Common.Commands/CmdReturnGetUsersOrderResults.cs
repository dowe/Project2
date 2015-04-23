﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetUsersOrderResults : Command
    {

        public ReadOnlyCollection<Order> Orders { get; private set; }

        public CmdReturnGetUsersOrderResults(List<Order> orders)
        {
            Orders = orders.AsReadOnly();
        }

    }
}
