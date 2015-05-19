using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;

namespace Server.ExtremeValueCheck
{
    class ExtremeValueChecker : IExtremeValueChecker
    {
        private bool extremeValue;

        public bool isExtreme(Test test)
        {
            if (test.ResultValue <= test.Analysis.ExtremeMinValue || test.ResultValue >= test.Analysis.ExtremeMaxValue)
            {
                extremeValue = true;
            } 
            return extremeValue;
        }
    }
}
