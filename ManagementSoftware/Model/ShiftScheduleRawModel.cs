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

        public event EventHandler Change;

        public ShiftScheduleRawModel()
        {
            _Data = new ShiftSchedule[2];
            _CurrentDataIndex = 0;
        }

        private void Notify()
        {
            Change(this, null);
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

        public ShiftSchedule CurrentData
        {
            get
            {
                return _Data[_CurrentDataIndex];
            }
        }

    }
}
