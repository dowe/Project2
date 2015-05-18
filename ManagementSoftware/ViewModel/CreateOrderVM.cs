using Common.Communication;
using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using ManagementSoftware.Helper;
using System.Globalization;
using Common.Util;

namespace ManagementSoftware.ViewModel
{
    public class CreateOrderVM : ViewModelBase
    {
        private IClientConnection _Connection;
        private List<Analysis> _AvaibleAnalysis;
        private Dictionary<String, List<Analysis>> _PatientTests; //Key = Patientid
        private string _CustomerUsername;
        private Address _CustomerAddress;
        private String _SelectedPatient;

        private MyListBox AvaibleAnalysisBox;

        public CreateOrderVM(IClientConnection _Connection)
        {
            this._Connection = _Connection;
            _AvaibleAnalysis = new List<Analysis>();
            _PatientTests = new Dictionary<String, List<Analysis>>();
            _CustomerUsername = "";

            _PatientTests.Add("Daniel1", new List<Analysis>());
            _PatientTests.Add("Daniel2", new List<Analysis>());
            _SelectedPatient = "Daniel1";

            new Thread(LoadAnalysis).Start();
        }

        public void SetBox(MyListBox box)
        {
            AvaibleAnalysisBox = box;
        }

        private void LoadAnalysis()
        {
            Command request = new CmdGetAnalyses();
            CmdReturnGetAnalyses response;
            response = _Connection.SendWait<CmdReturnGetAnalyses>(request);
            if (response != null)
            {
                AvaibleAnalysis = new List<Analysis>(response.Analyses);
            }

        }

        public List<Analysis> AvaibleAnalysis
        {
            get
            {
                return _AvaibleAnalysis;
            }
            set
            {
                _AvaibleAnalysis = value;
                RaisePropertyChanged();
            }
        }

        public void SelectedAnalysisChanged()
        {
            if (_SelectedPatient == null
                   || !_PatientTests.ContainsKey(_SelectedPatient)
                   || AvaibleAnalysisBox == null)
            {
                return;
            }

            List<Analysis> list = new List<Analysis>();

            foreach (Analysis item in AvaibleAnalysisBox.SelectedItems)
            {
                list.Add(item);
            }

            _PatientTests[_SelectedPatient] = list;
            RaisePropertyChanged(() => SelectedAnalysis);
        }

        public List<Analysis> SelectedAnalysis
        {
            get
            {
                if (_SelectedPatient == null
                    || !_PatientTests.ContainsKey(_SelectedPatient)
                    )
                {
                    return new List<Analysis>();
                }

                return _PatientTests[_SelectedPatient];
            }
        }

        public List<String> PatientIDs
        {
            get
            {
                return new List<String>(_PatientTests.Keys);
            }
        }

        public String SelectedPatient
        {
            get
            {
                return _SelectedPatient;
            }
            set
            {
                _SelectedPatient = value;
                RaisePropertyChanged();
                SelectAnalysis();
                RaisePropertyChanged(() => SelectedAnalysis);
            }
        }

        private void SelectAnalysis()
        {
            if (AvaibleAnalysisBox == null)
            {
                return;
            }

            

            if (_SelectedPatient == null
                    || !_PatientTests.ContainsKey(_SelectedPatient) )
            {
                AvaibleAnalysisBox.UnselectAll();
                return;
            }

            List<object> list = new List<object>();
            foreach (Analysis a in _PatientTests[_SelectedPatient])
            {
                list.Add(a);
            }
            
            if (list.Count > 0)
            {
                AvaibleAnalysisBox.SelectedItem = list[0];
            }
            AvaibleAnalysisBox.Select(list);
        }

        public string CustomerUsername
        {
            get
            {
                return _CustomerUsername;
            }
            set
            {
                _CustomerUsername = value;
                RaisePropertyChanged();
            }
        }
    }
}
