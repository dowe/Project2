using GalaSoft.MvvmLight;
using ManagementSoftware.ViewModel.Util;
using System.ComponentModel;

namespace ManagementSoftware.ViewModel
{
    public class RegisterCustomerVM : ViewModelBase
    {
        public object SMSWantedValues
        {
            get
            {
                return ViewModelUtil.EnumValues(typeof(SMSWanted));
            }
        }

        public object GenderTitleValues
        {
            get
            {
                return ViewModelUtil.EnumValues(typeof(GenderTitle));
            }
        }

        public virtual string LastName
        {
            get;
            set;
        }

        public virtual string FirstName
        {
            get;
            set;
        }

        public virtual string Label
        {
            get;
            set;
        }

        public virtual string Title
        {
            get;
            set;
        }

        public virtual string UserName
        {
            get;
            set;
        }

        public virtual string Password
        {
            get;
            set;
        }

        public virtual string MobileNumber
        {
            get;
            set;
        }

        public virtual bool SMSRequested
        {
            get;
            set;
        }

        public virtual string BankAccountOwner
        {
            get;
            set;
        }

        public virtual string IBAN
        {
            get;
            set;
        }

        public virtual string City
        {
            get;
            set;
        }

        public virtual string PostalCode
        {
            get;
            set;
        }

        public virtual string Street
        {
            get;
            set;
        }

    }

    public enum SMSWanted
    {
        [Description("Ja")]
        Yes,
        [Description("Nein")]
        No
    }

    public enum GenderTitle
    {
        [Description("Herr")]
        Mr,
        [Description("Frau")]
        Mrs
    }
}
