using Common.DataTransferObjects;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data
{
    public class TestData
    {
        public static Customer CreateTestData(ETitle title, ESMSRequested smsRequested)
        {

            Customer _Customer = new Customer();
            _Customer.Title = Util.CreateValuePair<ETitle>(title).Value;
            _Customer.FirstName = "Hans";
            _Customer.LastName = "Feil";
            _Customer.UserName = "hfeil";
            _Customer.Password = "asdf";
            _Customer.SMSRequested = smsRequested == ESMSRequested.Yes;
            _Customer.MobileNumber = "016212345";
            _Customer.Label = "Praxis ABC";
            _Customer.Address = new Address("abc 8", "77656", "Offenburg");
            _Customer.BankAccount = new BankAccount("12345-DE", "Hans feil");
            return _Customer;
        }
    }
}
