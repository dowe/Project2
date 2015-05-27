using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Documents;
using ManagementSoftware.Helper;
using ManagementSoftware.Model;
using GalaSoft.MvvmLight.Command;

namespace ManagementSoftware.ViewModel
{
    public class CreateOrderVM : ViewModelBase
    {

        private CreateOrderM model;
        private MyListBox AvaibleAnalysisBox;


        public CreateOrderVM(IClientConnection _Connection,
            IMessageBox _MessageBox)
        {
            model = new CreateOrderM(_Connection, _MessageBox);

            new Thread(LoadAnalysis).Start();

            AddPatientAction = new RelayCommand(AddPatient);
            RemovePatientAction = new RelayCommand(RemovePatient, CanRemovePatient);
            CancelOrderAction = new RelayCommand(CancelOrder);
            CreateOrderAction = new RelayCommand(CreateOrder, CanCreateOrder);
        }

        private bool CanRemovePatient()
        {
            return SelectedPatient != null;
        }

        private bool CanCreateOrder()
        {
            return model.Validate() == null;
        }

        public void SetBox(MyListBox box)
        {
            AvaibleAnalysisBox = box;
        }

        private void LoadAnalysis()
        {
            model.LoadAnalysis();
            RaisePropertyChanged(() => AvaibleAnalysis);
        }

        private void SelectAnalysis()
        {
            if (AvaibleAnalysisBox == null)
            {
                return;
            }

            AvaibleAnalysisBox.Select(model.SelectedAnalysis);
        }

        private void LoadCustomerAddress()
        {
            model.LoadCustomerAddress();
            RaisePropertyChanged(() => ValidationText);
            RaisePropertyChanged(() => CustomerAddressText);
        }

        private void RemovePatient()
        {
            model.RemovePatient();
            SelectedPatient = null;
            RaisePropertyChanged(() => ValidationText);
            RaisePropertyChanged(() => PatientIDs);
        }

        private void AddPatient()
        {
            SelectedPatient = model.AddPatient();
            RaisePropertyChanged(() => ValidationText);
            RaisePropertyChanged(() => PatientIDs);
            RaisePropertyChanged(() => NewPatientID);
        }

        private void CancelOrder()
        {
            model.CancelOrder();
            RemovePatientAction.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => SelectedAnalysis);
            RaisePropertyChanged(() => PatientIDs);
            RaisePropertyChanged(() => SelectedPatient);
            RaisePropertyChanged(() => CustomerUsername);
            RaisePropertyChanged(() => CustomerAddressText);
            RaisePropertyChanged(() => NewPatientID);
            RaisePropertyChanged(() => PatientIDText);
            RaisePropertyChanged(() => ValidationText);
        }

        private void CreateOrder()
        {
            model.CreateOrder();
        }

        public string ValidationText
        {
            get
            {
                CreateOrderAction.RaiseCanExecuteChanged();
                return model.Validate();
            }
        }

        public List<Analysis> AvaibleAnalysis
        {
            get
            {
                return model.AvaibleAnalysis;
            }
        }

        public void SelectedAnalysisChanged()
        {
            if (AvaibleAnalysisBox == null)
            {
                return;
            }

            List<Analysis> list = new List<Analysis>();
            foreach (Analysis item in AvaibleAnalysisBox.SelectedItems)
            {
                list.Add(item);
            }

            model.SelectedAnalysis = list;
            RaisePropertyChanged(() => ValidationText);
            RaisePropertyChanged(() => SelectedAnalysis);
        }

        public List<Analysis> SelectedAnalysis
        {
            get
            {
                SelectAnalysis();
                return model.SelectedAnalysis;
            }
        }

        public List<String> PatientIDs
        {
            get
            {
                return model.PatientIDs();
            }
        }

        public String SelectedPatient
        {
            get
            {
                return model.SelectedPatient;
            }
            set
            {
                model.SelectedPatient = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => SelectedAnalysis);
                RaisePropertyChanged(() => PatientIDText);
                RemovePatientAction.RaiseCanExecuteChanged();
            }
        }

        public string CustomerUsername
        {
            get
            {
                return model.CustomerUsername;
            }
            set
            {
                model.CustomerUsername = value;
                new Thread(LoadCustomerAddress).Start();
                RaisePropertyChanged();
                RaisePropertyChanged(() => ValidationText);
            }
        }

        public string CustomerAddressText
        {
            get
            {
                return model.CustomerAddressText;
            }
        }

        public string NewPatientID
        {
            get
            {
                return model.NewPatientID;
            }
            set
            {
                model.NewPatientID = value;
                RaisePropertyChanged();
            }
        }

        public string PatientIDText
        {
            get
            {
                return model.PatientIDText;
            }
        }

        public RelayCommand AddPatientAction
        {
            get;
            set;
        }

        public RelayCommand RemovePatientAction
        {
            get;
            set;
        }

        public RelayCommand CancelOrderAction
        {
            get;
            set;
        }

        public RelayCommand CreateOrderAction
        {
            get;
            set;
        }
    }
}
