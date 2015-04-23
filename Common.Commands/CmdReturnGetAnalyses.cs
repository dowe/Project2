using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetAnalyses : Command
    {

        public ReadOnlyCollection<Analysis> Analyses { get; private set; }

        public CmdReturnGetAnalyses(List<Analysis> analyses)
        {
            Analyses = analyses.AsReadOnly();
        }

    }
}
