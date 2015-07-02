using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication;
using Common.Commands;
using Common.Communication.Server;
using Server.DatabaseCommunication;
using Server.DistanceCalculation;
using Common.DataTransferObjects;
using Common.Util;

namespace Server.CmdHandler
{
    class CmdRegisterCustomerHandler : CommandHandler<CmdRegisterCustomer>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;
        private ILocalServerData data;

        public CmdRegisterCustomerHandler(
            IServerConnection connection,
            IDatabaseCommunicator db,
            ILocalServerData data)
        {
            this.connection = connection;
            this.db = db;
            this.data = data;
        }

        protected override void Handle(CmdRegisterCustomer request, string connectionIdOrNull)
        {
            Customer customer = request.Customer;
            string errorMessage = Validate(customer);
            string responseMessage = errorMessage;
            bool success = false;

            if (errorMessage == null)
            {
                customer.GpsPosition = Geolocation.ConvertToGPS(new DistanceMatrixAddress(customer.Address));

                db.StartTransaction();
                db.CreateCustomer(customer);
                db.EndTransaction(TransactionEndOperation.SAVE);

                success = true;
                responseMessage = "Kunde angelegt";
            }

            Response(request.Id, connectionIdOrNull, success, responseMessage);
        }

        private void Response(Guid id, string connectionId, bool success, string message)
        {
            Command response = new CmdReturnRegisterCustomer(id, success, message);
            connection.Unicast(response, connectionId);
        }

        private string Validate(Customer customer)
        {
            //test validate fields
            string error = ValidateNotNullFields(customer);
            if (error != null)
            {
                return error;
            }

            //test Customer [KEY] unique
            db.StartTransaction();
            if (db.GetCustomer(customer.UserName) != null)
            {
                error = "Benutzername bereits vergeben";
            }
            Customer c = db.GetAllCustomer((x) => x.BankAccount.IBAN.Equals(customer.BankAccount.IBAN)).FirstOrDefault();
            if (c != null)
            {
                customer.BankAccount = c.BankAccount;
            }
            db.EndTransaction(TransactionEndOperation.READONLY);
            if (error != null)
            {
                return error;
            }


            //test DistanceCalculation < 100km
            DistanceContainer container;
            try
            {
                container = DistanceCalculation.DistanceCalculation.CalculateDistanceInKm(data.ZmsAddress, customer.Address);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (container.Distance > 100.0)
            {
                return "Kunde weiter als 100km entfernt";
            }

            //SET customer.TwoWayRoadCostInEuro
            float f = container.Distance * 2.0F * data.RoadCostInEuroPerKm;
            float truncated = (float)(Math.Truncate((double)f * 100.0) / 100.0);
            customer.TwoWayRoadCostInEuro = truncated;

            return null;
        }

        private string ValidateNotNullFields(Customer customer)
        {
            string error = null;
            error = ValidateNotNull(customer, error, "Kundenobjekt fehlt");
            error = ValidateNotNull(customer.LastName, error, "Name fehlt");
            error = ValidateNotNull(customer.FirstName, error, "Vorname fehlt");
            error = ValidateNotNull(customer.Label, error, "Bezeichnung fehlt");
            error = ValidateNotNull(customer.Title, error, "Anrede fehlt");
            error = ValidateNotNull(customer.UserName, error, "Benutzername fehlt");
            error = ValidateNotNull(customer.Password, error, "Passwort fehlt");
            error = ValidateNotNull(customer.MobileNumber, error, "Handynummer fehlt");
            error = ValidateNotNull(customer.Address, error, "Adressobjekt fehlt");
            error = ValidateNotNull(customer.Address.Street, error, "Straße+Hnr fehlt");
            error = ValidateNotNull(customer.Address.City, error, "Ort fehlt");
            error = ValidateNotNull(customer.Address.PostalCode, error, "PLZ fehlt");
            error = ValidateNotNull(customer.BankAccount, error, "Bankobjekt fehlt");
            error = ValidateNotNull(customer.BankAccount.AccountOwner, error, "Kontoinhaber fehlt");
            error = ValidateNotNull(customer.BankAccount.IBAN, error, "IBAN fehlt");

            return error;
        }

        private string ValidateNotNull(string text, string previousMessage, string newMessage)
        {
            if (previousMessage != null)
            {
                return previousMessage;
            }

            if (String.IsNullOrEmpty(text))
            {
                return newMessage;
            }

            return null;
        }

        private string ValidateNotNull(object obj, string previousMessage, string newMessage)
        {
            if (previousMessage != null)
            {
                return previousMessage;
            }

            if (obj == null)
            {
                return newMessage;
            }

            return null;
        }
    }
}
