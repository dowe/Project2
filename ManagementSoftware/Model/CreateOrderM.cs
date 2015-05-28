using Common.Commands;
using Common.Communication;
using Common.Communication.Client;
using Common.DataTransferObjects;
using ManagementSoftware.Helper;
using System;
using System.Collections.Generic;

namespace ManagementSoftware.Model
{
    public class CreateOrderM
    {
        public static readonly string FAILURE_LOAD_CUSTOMER_ADDRESS = "Fehler beim versenden der Anfrage zur Kundenadresse \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut";
        public static readonly string SUCCESS_CREATE_ORDER_PREFIX = "Bestellung erstellt!\nBestellungs ID: ";
        public static readonly string FAILURE_CREATE_ORDER = "Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut";

        public static readonly string INVALID_ADDRESS = "Invalider Kunde\n     Kundenadresse unbekannt";
        public static readonly string INVALID_USERNAME = "Kunden ID fehlt";
        public static readonly string INVALID_PATIENT_COUNT = "Kein Patient hinzugefügt";
        public static readonly string INVALID_TEST_COUNT = "Einem oder mehreren Patienten\n   wurde keine Untersuchung zugewiesen";


        private IClientConnection _Connection;
        private Dictionary<String, List<Analysis>> _PatientTests; //Key = Patientid
        private IMessageBox _MessageBox;

        public CreateOrderM(
            IClientConnection _Connection,
            IMessageBox _MessageBox)
        {
            this._MessageBox = _MessageBox;
            this._Connection = _Connection;
            this._PatientTests = new Dictionary<String, List<Analysis>>();

            CustomerAddress = null;
            AvaibleAnalysis = new List<Analysis>();
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
                _MessageBox.Show(FAILURE_LOAD_CUSTOMER_ADDRESS);
            }
        }

        public void LoadAnalysis()
        {
            Command request = new CmdGetAnalyses();
            CmdReturnGetAnalyses response;
            response = _Connection.SendWait<CmdReturnGetAnalyses>(request);
            if (response != null)
            {
                AvaibleAnalysis = new List<Analysis>(response.Analyses);
            }
        }

        public string CustomerAddressText
        {
            get
            {
                if (CustomerAddress != null)
                {
                    return String.Format("{0}\n{1} {2}",
                        CustomerAddress.Street,
                        CustomerAddress.PostalCode,
                        CustomerAddress.City);
                }
                else
                {
                    return "Kunde nicht gefunden.";
                }
            }
        }

        public List<Analysis> SelectedAnalysis
        {
            get
            {
                if (SelectedPatient == null
                    || !_PatientTests.ContainsKey(SelectedPatient))
                {
                    return new List<Analysis>();
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
                    return "Keine Patienten ID ausgewählt";
                }
                else
                {
                    return String.Format("Für [{0}]", SelectedPatient);
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
                _PatientTests.Add(patientID, new List<Analysis>());
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

            Command request = new CmdAddOrder(_PatientTests, CustomerUsername);
            CmdReturnAddOrder response;
            response = _Connection.SendWait<CmdReturnAddOrder>(request);
            if (response != null)
            {
                _MessageBox.Show(SUCCESS_CREATE_ORDER_PREFIX
                    + response.CreatedOrderId);
            }
            else
            {
                _MessageBox.Show(FAILURE_CREATE_ORDER);
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

            foreach (KeyValuePair<String, List<Analysis>> item in _PatientTests)
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

        public Address CustomerAddress { get; set; }
        public String SelectedPatient { get; set; }
        public string NewPatientID { get; set; }
        public List<Analysis> AvaibleAnalysis { get; set; }
        public string CustomerUsername { get; set; }

    }
}
