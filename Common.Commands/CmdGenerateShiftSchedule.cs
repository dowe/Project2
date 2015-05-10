using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdGenerateShiftSchedule : Command
    {
        public CmdGenerateShiftSchedule()
            : this(GenerateMonthMode.DEFAULT_NEXT_MONTH)
        {
            
        }

        public CmdGenerateShiftSchedule(GenerateMonthMode mode)
        {
            this.Mode = mode;
        }

        public GenerateMonthMode Mode { get; private set; }
    }

    public enum GenerateMonthMode
    {
        IMMEDIATELY_CURRENT_MONTH,
        IMMEDIATELY_NEXT_MONTH,
        DEFAULT_NEXT_MONTH
    }
}
