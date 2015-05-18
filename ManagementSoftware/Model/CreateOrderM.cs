using Common.Commands;
using Common.Communication;
using Common.Communication.Client;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ManagementSoftware.Model
{
    public class CreateOrderM
    {
        private IClientConnection _Connection;
        private Dictionary<String, List<Analysis>> _PatientTests; //Key = Patientid

        public CreateOrderM(IClientConnection _Connection)
        {
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
                MessageBox.Show("Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
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
                    return CustomerAddress.Street + "\n"
                      + CustomerAddress.PostalCode + " " + CustomerAddress.City;
                }
                else
                {
                    return "Kundennummer nicht gefunden.";
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
                    return "Für [" + SelectedPatient + "]";
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
            string validateOrder = Validate();
            if (validateOrder != null)
            {
                MessageBox.Show(validateOrder);
                return;
            }

            Command request = new CmdAddOrder(_PatientTests, CustomerUsername);
            CmdReturnAddOrder response;
            response = _Connection.SendWait<CmdReturnAddOrder>(request);
            if (response != null)
            {
                MessageBox.Show("Bestellung erstellt!"
                    + "\nBestellungs ID: " + response.CreatedOrderId);
            }
            else
            {
                MessageBox.Show("Fehler beim versenden der Anfrage zur Registrierung des Kunden. \n - Überprüfen Sie ihre Internetverbindung\n - Versuchen Sie es später erneut");
            }
        }

        private string Validate()
        {
            if (CustomerUsername.Trim().Length == 0)
            {
                return "Kunden ID fehlt";
            }

            if (CustomerAddress == null)
            {
                return "Invalider Kunde"
                    + "\n Kundenadresse unbekannt";
            }

            if (_PatientTests.Count == 0)
            {
                return "Keine Patienten hinzugefügt";
            }

            foreach (KeyValuePair<String, List<Analysis>>  item in _PatientTests)
            {
                if (item.Value.Count == 0)
                {
                    return "Einem oder mehreren Patienten"
                    + "\nwurde keine Untersuchung zugewiesen";
                }
            }

            return null;
        }

        public Address CustomerAddress { get; set; }
        public String SelectedPatient { get; set; }
        public string NewPatientID { get; set; }
        public List<Analysis> AvaibleAnalysis { get; set; }
        public string CustomerUsername { get; set; }

    }
}
