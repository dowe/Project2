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
using System.Windows;
using ManagementSoftware.Model;
using GalaSoft.MvvmLight.Command;

namespace ManagementSoftware.ViewModel
{
    public class CreateOrderVM : ViewModelBase
    {

        private CreateOrderM model;
        private MyListBox AvaibleAnalysisBox;


        public CreateOrderVM(IClientConnection _Connection)
        {
            model = new CreateOrderM(_Connection);

            new Thread(LoadAnalysis).Start();

            AddPatientAction = new RelayCommand(AddPatient);
            RemovePatientAction = new RelayCommand(RemovePatient, CanRemovePatient);
            CancelOrderAction = new RelayCommand(CancelOrder);
            CreateOrderAction = new RelayCommand(CreateOrder);//TODO canExecute
        }

        private bool CanRemovePatient()
        {
            return SelectedPatient != null;
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
            RaisePropertyChanged(() => CustomerAddressText);
        }

        private void RemovePatient()
        {
            model.RemovePatient();
            SelectedPatient = null;
            RaisePropertyChanged(() => PatientIDs);
        }

        private void AddPatient()
        {
            SelectedPatient = model.AddPatient();
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
        }

        private void CreateOrder()
        {
            model.CreateOrder();
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
