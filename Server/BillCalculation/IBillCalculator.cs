using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.BillCalculation
{
   public interface IBillCalculator
    {
       Bill CalculateBill(List<Test> tests);
    }
}
