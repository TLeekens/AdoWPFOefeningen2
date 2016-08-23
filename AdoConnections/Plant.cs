using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoConnections
{
    public class Plant
    {
        private Int32 plantNrValue;
        private String naamValue;
        private Int32 soortNrValue;
        private Int32 levnrValue;
        private String kleurValue;
        private Decimal verkoopPrijsValue;

        public Int32 PlantNr
        {
            get
            {
                return plantNrValue;
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
            }
        }
        public Int32 SoortNr
        {
            get
            {
                return soortNrValue;
            }
            set
            {
                soortNrValue = value;
            }
        }
        public Int32 Levnr
        {
            get
            {
                return levnrValue;
            }
            set
            {
                levnrValue = value;
            }
        }
        public String Kleur
        {
            get
            {
                return kleurValue;
            }
            set
            {
                kleurValue = value;
            }
        }
        public Decimal VerkoopPrijs
        {
            get
            {
                return verkoopPrijsValue;
            }
            set
            {
                verkoopPrijsValue = value;
            }
        }

        public Plant (Int32 plantNr, String naam, Int32 soortNr, Int32 levnr, String kleur, Decimal verkoopPrijs)
        {
            plantNrValue = plantNr;
            Naam = naam;
            SoortNr = soortNr;
            Levnr = levnr;
            Kleur = kleur;
            VerkoopPrijs = verkoopPrijs;
        }

        public override string ToString()
        {
            return Naam;
        }
    }
}
