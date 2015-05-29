using Common.Commands;
using Common.Communication;
using Common.Communication.Client;
using Common.DataTransferObjects;
using ManagementSoftware.Helper;
using System;
using System.Collections.Generic;

namespace ManagementSoftware.Model
{
    public class CreateOrderModel
    {
        public static readonly string NORESPONSE_LOAD_CUSTOMER_ADDRESS = "Fehler beim versenden der Anfrage zur Kundenadresse \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut";
        public static readonly string SUCCESS_CREATE_ORDER_PREFIX = "Bestellung erstellt!\nBestellungs ID: ";
        public static readonly string NORESPONSE_CREATE_ORDER = "Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut";

        public static readonly string INVALID_ADDRESS = "Invalider Kunde\n     Kundenadresse unbekannt";
        public static readonly string INVALID_USERNAME = "Kunden ID fehlt";
        public static readonly string INVALID_PATIENT_COUNT = "Kein Patient hinzugefügt";
        public static readonly string INVALID_TEST_COUNT = "Einem oder mehreren Patienten\n   wurde keine Untersuchung zugewiesen";

        public static readonly string UNSELECTED_PATIENT_TEXT = "Keine Patienten ID ausgewählt";
        public static readonly string SELECTED_PATIENT_PATTERN = "Für [{0}]";

        public static readonly string UNKNOWN_CUSTOMER_ADDRESS_TEXT = "Kunde nicht gefunden.";
        public static readonly string CUSTOMER_ADRESS_TEXT_PATTERN = "{0}\n{1} {2}";

        private IClientConnection _Connection;
        private Dictionary<String, List<AnalysisModel>> _PatientTests; //Key = Patientid
        private IMessageBox _MessageBox;
        
        
        

        public CreateOrderModel(
            IClientConnection _Connection,
            IMessageBox _MessageBox)
        {
            this._MessageBox = _MessageBox;
            this._Connection = _Connection;
            this._PatientTests = new Dictionary<String, List<AnalysisModel>>();

            CustomerAddress = null;
            AvaibleAnalysis = new List<AnalysisModel>();
            CustomerUsername = "";
            NewPatientID = "";
            SelectedPatient = null;
        }

        public void LoadCustomerAddress()
        {
            CustomerAddress = null;
            Command request = new CmdGetCustomerAddress(CustomerUsername);
            CmdReturnGetCustomerAddress response;
            response = _Connection.SendWait<CmdReturnGetCustomerAddress>(request);
            if (response != null)
            {
                CustomerAddress = response.CustomerAddress;
            }
            else
            {
                _MessageBox.Show(NORESPONSE_LOAD_CUSTOMER_ADDRESS);
            }
        }

        public void LoadAnalysis()
        {
            Command request = new CmdGetAnalyses();
            CmdReturnGetAnalyses response;
            response = _Connection.SendWait<CmdReturnGetAnalyses>(request);
            if (response != null)
            {
                List<AnalysisModel> list = new List<AnalysisModel>();
                foreach (Analysis obj in response.Analyses)
                {
                    list.Add(new AnalysisModel(obj));
                }
                AvaibleAnalysis = list;
            }
        }

        public string CustomerAddressText
        {
            get
            {
                if (CustomerAddress != null)
                {
                    return String.Format(CUSTOMER_ADRESS_TEXT_PATTERN,
                        CustomerAddress.Street,
                        CustomerAddress.PostalCode,
                        CustomerAddress.City);
                }
                else
                {
                    return UNKNOWN_CUSTOMER_ADDRESS_TEXT;
                }
            }
        }

        public List<AnalysisModel> SelectedAnalysis
        {
            get
            {
                if (SelectedPatient == null
                    || !_PatientTests.ContainsKey(SelectedPatient))
                {
                    return new List<AnalysisModel>();
                }

                return _PatientTests[SelectedPatient];
            }
            set
            {
                if (SelectedPatient == null
                   || !_PatientTests.ContainsKey(SelectedPatient))
                {
                    return;
                }

                _PatientTests[SelectedPatient] = value;
            }
        }

        public List<String> PatientIDs()
        {
            return new List<String>(_PatientTests.Keys);
        }

        public string PatientIDText
        {
            get
            {
                if (SelectedPatient == null)
                {
                    return UNSELECTED_PATIENT_TEXT;
                }
                else
                {
                    return String.Format(SELECTED_PATIENT_PATTERN, SelectedPatient);
                }
            }
        }

        public void RemovePatient()
        {
            if (SelectedPatient != null)
            {
                _PatientTests.Remove(SelectedPatient);
            }
        }

        public string AddPatient()
        {
            string patientID = NewPatientID;
            if (patientID != null
                && patientID.Trim().Length > 0
                && !_PatientTests.ContainsKey(patientID))
            {
                _PatientTests.Add(patientID, new List<AnalysisModel>());
                NewPatientID = "";
                return patientID;
            }
            return SelectedPatient;
        }

        public void CancelOrder()
        {
            _PatientTests.Clear();
            CustomerAddress = null;
            SelectedPatient = null;
            NewPatientID = "";
            CustomerUsername = "";
        }

        public void CreateOrder()
        {

            Dictionary<String, List<Analysis>> _dict = new Dictionary<String, List<Analysis>>();

            foreach (KeyValuePair<String, List<AnalysisModel>> item in _PatientTests)
            {
                List<Analysis> list = new List<Analysis>();
                foreach (AnalysisModel a in item.Value)
                {
                    list.Add(a.Analysis);
                }
                _dict.Add(item.Key, list);
            }


            Command request = new CmdAddOrder(_dict, CustomerUsername);
            CmdReturnAddOrder response;
            response = _Connection.SendWait<CmdReturnAddOrder>(request);
            if (response != null)
            {
                _MessageBox.Show(SUCCESS_CREATE_ORDER_PREFIX
                    + response.CreatedOrderId);
            }
            else
            {
                _MessageBox.Show(NORESPONSE_CREATE_ORDER);
            }
        }

        public List<string> Validate()
        {
            List<string> message = new List<string>();
            if (CustomerUsername.Trim().Length == 0)
            {
                append(message, INVALID_USERNAME);
            }

            if (CustomerAddress == null)
            {
                append(message, INVALID_ADDRESS);
            }

            if (_PatientTests.Count == 0)
            {
                append(message, INVALID_PATIENT_COUNT);
            }

            foreach (KeyValuePair<String, List<AnalysisModel>> item in _PatientTests)
            {
                if (item.Value.Count == 0)
                {
                    append(message, INVALID_TEST_COUNT);
                }
            }

            return message;
        }

        private void append(List<string> l, string message)
        {
            l.Add(message);
        }

        public float Cost
        {
            get
            {
                float f = 0.0F;

                foreach (KeyValuePair<String, List<AnalysisModel>> item in _PatientTests)
                {
                    foreach (AnalysisModel a in item.Value)
                    {
                        f += a.Analysis.PriceInEuro;
                    }
                }

                return f;
            }
        }

        public Address CustomerAddress { get; set; }
        public String SelectedPatient { get; set; }
        public string NewPatientID { get; set; }
        public List<AnalysisModel> AvaibleAnalysis { get; set; }
        public string CustomerUsername { get; set; }


        
    }
}
