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
    public class CmdReturnAnalyses : Command
    {

        public CmdReturnAnalyses(List<Analysis> analyses)
        {
            Analyses = analyses.AsReadOnly();
        }

        public ReadOnlyCollection<Analysis> Analyses { get; private set; }
    }
}
