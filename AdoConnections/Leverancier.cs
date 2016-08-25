using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoConnections
{
    public class Leverancier
    {
        private Int32 levNrValue;
        private String naamValue;
        private String adresValue;
        private String postNrValue;
        private String woonplaatsValue;
        private bool changedValue;

        public Int32 LevNr
        {
            get
            {
                return levNrValue;
            }
        }
        public String Naam
        {
            get
            {
                return naamValue;
            }
            set
            {
                naamValue = value;
                Changed = true;
            }
        }
        public String Adres
        {
            get
            {
                return adresValue;
            }
            set
            {
                adresValue = value;
                Changed = true;
            }
        }
        public String PostNr
        {
            get
            {
                return postNrValue;
            }
            set
            {
                postNrValue = value;
                Changed = true;
            }
        }
        public String Woonplaats
        {
            get
            {
                return woonplaatsValue;
            }
            set
            {
                woonplaatsValue = value;
                Changed = true;
            }
        }
        public bool Changed
        {
            get
            {
                return changedValue;
            }
            set
            {
                changedValue = value;
            }
        }

        public Leverancier()
        {

        }

        public Leverancier (Int32 levNr, String naam, String adres, String postNr, String woonplaats)
        {
            levNrValue = levNr;
            Naam = naam;
            Adres = adres;
            PostNr = postNr;
            Woonplaats = woonplaats;
            Changed = false;
        }
    }
}
