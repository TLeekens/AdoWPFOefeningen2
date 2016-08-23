using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoConnections
{
    public class SoortManager
    {
        public List<Soort> GetSoortenMetOptieAllemaalVoorop()
        {
            List<Soort> soorten = new List<Soort>();
            soorten.Add(new Soort("<allemaal>", 0));

            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comSoorten = conTuincentrum.CreateCommand())
                {
                    comSoorten.CommandType = CommandType.Text;
                    comSoorten.CommandText = "select SoortNr, Soort from Soorten order by Soort";

                    conTuincentrum.Open();

                    using (var rdrSoorten = comSoorten.ExecuteReader())
                    {
                        var soortPos = rdrSoorten.GetOrdinal("soort");
                        var SoortNrPos = rdrSoorten.GetOrdinal("soortnr");

                        while (rdrSoorten.Read())
                        {
                            soorten.Add(new Soort(rdrSoorten.GetString(soortPos), rdrSoorten.GetInt32(SoortNrPos)));
                        }
                    }
                }
            }
            return soorten;
        }
    }
}
