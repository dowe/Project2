using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class TestEntryModel
    {
        private String _AlarmState, _BringDate, _CustomerLabel, _OrderID, _PatientID, _SampleTypeAndAnalysis, _TestID, _TestState;
        private bool _SampleCollected;

        public bool SampleCollected
        {
            get { return _SampleCollected; }
            set { _SampleCollected = value; }
        }

        public String TestState
        {
            get { return _TestState; }
            set { _TestState = value; }
        }

        public String TestID
        {
            get { return _TestID; }
            set { _TestID = value; }
        }

        public String SampleTypeAndAnalysis
        {
            get { return _SampleTypeAndAnalysis; }
            set { _SampleTypeAndAnalysis = value; }
        }

        public String PatientID
        {
            get { return _PatientID; }
            set { _PatientID = value; }
        }

        public String OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        public String CustomerLabel
        {
            get { return _CustomerLabel; }
            set { _CustomerLabel = value; }
        }

        public String BringDate
        {
            get { return _BringDate; }
            set { _BringDate = value; }
        }

        public String AlarmState
        {
            get { return _AlarmState; }
            set { _AlarmState = value; }
        }

       
        


    }
}
