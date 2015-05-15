using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;

namespace Server.ExtremeValueCheck
{
    public interface IExtremeValueChecker
    {
        bool isExtreme(Test test);
    }
}
