using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdAddOrder : Command
    {

        public Dictionary<String, List<Analysis>> PatientTests; //Key = Patientid
        public string CustomerUsername { get; private set; }

        public CmdAddOrder(Dictionary<String, List<Analysis>> tests, string customerUsername)
        {
            PatientTests = tests;
            CustomerUsername = customerUsername;
        }

    }
}
