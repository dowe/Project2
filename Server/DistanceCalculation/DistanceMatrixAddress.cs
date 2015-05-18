using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;

namespace Server.DistanceCalculation
{
    public class DistanceMatrixAddress : IDistanceMatrixPlace
    {

        private Address adaptee = null;

        public DistanceMatrixAddress(Address adaptee)
        {
            this.adaptee = adaptee;
        }

        public string FormatAsDistanceMatrixPlace()
        {
            return string.Format("{0}+{1}+{2}", adaptee.Street, adaptee.PostalCode, adaptee.City); 
        }

    }
}
