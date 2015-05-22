using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public class TestDetailModel
    {
        TestEntryModel _Entry;
        String _BestellDatum, _Telefon, _KundenAdresse, _Grenzwerte, _Resultat, _Einheit;

        public TestEntryModel Entry
        {
            get { return _Entry; }
            set { _Entry = value; }
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
