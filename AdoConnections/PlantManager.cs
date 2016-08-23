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
    }
}
