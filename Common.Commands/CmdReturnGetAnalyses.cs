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
    public class CmdReturnGetAnalyses : ResponseCommand
    {

        public ReadOnlyCollection<Analysis> Analyses { get; private set; }

        public CmdReturnGetAnalyses(Guid requestId, List<Analysis> analyses) : base(requestId)
        {
            Analyses = analyses.AsReadOnly();
        }

    }
}
