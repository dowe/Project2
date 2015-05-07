using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    interface IBillCalculator
    {
       String CalculateBill(List<Test> tests);
    }
}
