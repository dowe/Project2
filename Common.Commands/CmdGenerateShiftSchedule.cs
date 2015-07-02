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

        public DateTime MonthForShiftSchedule { get; set; }

        public CmdGenerateShiftSchedule()
        {
            MonthForShiftSchedule = DateTime.Now;
        }

        public CmdGenerateShiftSchedule(DateTime monthForShiftSchedule)
        {
            MonthForShiftSchedule = monthForShiftSchedule;
        }
    }
}
