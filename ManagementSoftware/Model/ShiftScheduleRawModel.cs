using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class ShiftScheduleRawModel
    {
        private ShiftSchedule[] _Data;
        private int _CurrentDataIndex;

        public event EventHandler<ShiftScheduleRawModelChangeEventArgs> Change;

        public ShiftScheduleRawModel()
        {
            //TODO init
        }

        private void Notify()
        {
            Change(this, new ShiftScheduleRawModelChangeEventArgs { Data = _Data, CurrentDataIndex = _CurrentDataIndex });
        }

        public ShiftSchedule[] Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                Notify();
            }
        }

        public int CurrentDataIndex
        {
            get
            {
                return _CurrentDataIndex;
            }
            set
            {
                _CurrentDataIndex = value;
                Notify();
            }
        }

    }

    public class ShiftScheduleRawModelChangeEventArgs : EventArgs
    {
        public ShiftSchedule[] Data
        {
            get;
            set;
        }

        public int CurrentDataIndex
        {
            get;
            set;
        }

    }
}
