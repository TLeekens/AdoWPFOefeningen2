using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace AdoConnections
{
    public class PlantManager
    {
        public List<String> GetPlantenNamen(int soortNr)
        {
            List<String> planten = new List<string>();
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comPlanten = conTuincentrum.CreateCommand())
                {
                    comPlanten.CommandType = CommandType.Text;
                    if (soortNr == 0)
                    {
                        comPlanten.CommandText = "select naam from planten order by naam";
                    }
                    else
                    {
                        comPlanten.CommandText = "select naam from planten where soortnr=@soortnr order by naam";
                    }

                    var parSoortNr = comPlanten.CreateParameter();
                    parSoortNr.ParameterName = "@soortnr";
                    parSoortNr.Value = soortNr;
                    comPlanten.Parameters.Add(parSoortNr);

                    conTuincentrum.Open();

                    using (var rdrPlanten = comPlanten.ExecuteReader())
                    {
                        while (rdrPlanten.Read())
                        {
                            planten.Add(rdrPlanten["naam"].ToString());
                        }
                    }
                }
            }
            return planten;
        }

        public List<Plant> GetPlanten(int soortNr)
        {
            List<Plant> planten = new List<Plant>();
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comPlanten = conTuincentrum.CreateCommand())
                {
                    comPlanten.CommandType = CommandType.Text;
                    if (soortNr == 0)
                    {
                        comPlanten.CommandText = "select * from planten order by naam";
                    }
                    else
                    {
                        comPlanten.CommandText = "select * from planten where soortnr=@soortnr order by naam";
                    }

                    var parSoortNr = comPlanten.CreateParameter();
                    parSoortNr.ParameterName = "@soortnr";
                    parSoortNr.Value = soortNr;
                    comPlanten.Parameters.Add(parSoortNr);

                    conTuincentrum.Open();

                    using (var rdrPlanten = comPlanten.ExecuteReader())
                    {
                        Int32 posPlantNr = rdrPlanten.GetOrdinal("PlantNr");
                        Int32 posNaam = rdrPlanten.GetOrdinal("Naam");
                        Int32 posSoortNr = rdrPlanten.GetOrdinal("SoortNr");
                        Int32 posLevnr = rdrPlanten.GetOrdinal("Levnr");
                        Int32 posKleur = rdrPlanten.GetOrdinal("Kleur");
                        Int32 posVerkoopPrijs = rdrPlanten.GetOrdinal("VerkoopPrijs");

                        while (rdrPlanten.Read())
                        {
                            planten.Add(new Plant(rdrPlanten.GetInt32(posPlantNr), rdrPlanten.GetString(posNaam), rdrPlanten.GetInt32(posSoortNr), rdrPlanten.GetInt32(posLevnr), rdrPlanten.GetString(posKleur), rdrPlanten.GetDecimal(posVerkoopPrijs)));
                        }
                    }
                }
            }
            return planten;
        }

        public void SchrijfWijzigingen (List<Plant> planten)
        {
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comWijzigingen = conTuincentrum.CreateCommand())
                {
                    comWijzigingen.CommandType = CommandType.Text;
                    comWijzigingen.CommandText = "update planten set Kleur=@kleur, VerkoopPrijs=@verkoopprijs where PlantNr=@plantnr";

                    var parKleur = comWijzigingen.CreateParameter();
                    parKleur.ParameterName = "@kleur";
                    comWijzigingen.Parameters.Add(parKleur);

                    var parPrijs = comWijzigingen.CreateParameter();
                    parPrijs.ParameterName = "@verkoopprijs";
                    comWijzigingen.Parameters.Add(parPrijs);

                    var parPlantNr = comWijzigingen.CreateParameter();
                    parPlantNr.ParameterName = "@plantnr";
                    comWijzigingen.Parameters.Add(parPlantNr);

                    conTuincentrum.Open();

                    foreach (var eenPlant in planten)
                    {
                        parPlantNr.Value = eenPlant.PlantNr;
                        parKleur.Value = eenPlant.Kleur;
                        parPrijs.Value = eenPlant.VerkoopPrijs;

                        comWijzigingen.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
