using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class TestDetailModel
    {

        String _BestellDatum, _Telefon, _KundenAdresse, _Grenzwerte, _Resultat, _Einheit, _TestID, _BringDatum;

        public String BringDatum
        {
            get { return _BringDatum; }
            set { _BringDatum = value; }
        }

        public String TestID
        {
            get { return _TestID; }
            set { _TestID = value; }
        }

      
        public String Einheit
        {
            get { return _Einheit; }
            set { _Einheit = value; }
        }

        public String Resultat
        {
            get { return _Resultat; }
            set { _Resultat = value; }
        }

        public String Grenzwerte
        {
            get { return _Grenzwerte; }
            set { _Grenzwerte = value; }
        }

        public String KundenAdresse
        {
            get { return _KundenAdresse; }
            set { _KundenAdresse = value; }
        }

        public String Telefon
        {
            get { return _Telefon; }
            set { _Telefon = value; }
        }

        public String BestellDatum
        {
            get { return _BestellDatum; }
            set { _BestellDatum = value; }
        }

    }
}
