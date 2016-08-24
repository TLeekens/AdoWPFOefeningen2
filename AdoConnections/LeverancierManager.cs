using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace AdoConnections
{
    public class LeverancierManager
    {
        public ObservableCollection<Leverancier> GetLeveranciersVolgensNaam(String postNr)
        {
            ObservableCollection<Leverancier> leveranciers = new ObservableCollection<Leverancier>();
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comLeverancier = conTuincentrum.CreateCommand())
                {
                    comLeverancier.CommandType = CommandType.Text;
                    comLeverancier.CommandText = String.Format("select * from leveranciers{0} order by naam", postNr == "<alles>" ? "" : " where postnr=@postnr");

                    var parPostNr = comLeverancier.CreateParameter();
                    parPostNr.ParameterName = "@postnr";
                    parPostNr.Value = postNr;
                    comLeverancier.Parameters.Add(parPostNr);

                    conTuincentrum.Open();

                    using (var rdrLeverancier = comLeverancier.ExecuteReader())
                    {
                        Int32 posLevNr = rdrLeverancier.GetOrdinal("LevNr");
                        Int32 posNaam = rdrLeverancier.GetOrdinal("Naam");
                        Int32 posAdres = rdrLeverancier.GetOrdinal("Adres");
                        Int32 posPostNr = rdrLeverancier.GetOrdinal("PostNr");
                        Int32 posWoonplaats = rdrLeverancier.GetOrdinal("Woonplaats");

                        while (rdrLeverancier.Read())
                        {
                            leveranciers.Add(new Leverancier(rdrLeverancier.GetInt32(posLevNr), rdrLeverancier.GetString(posNaam), rdrLeverancier.GetString(posAdres), rdrLeverancier.GetString(posPostNr), rdrLeverancier.GetString(posWoonplaats)));
                        }
                    }
                }
            }
            return leveranciers;
        }

        public List<String> GetPostNrsMetAllesVoorop()
        {
            List<String> postnummers = new List<string>();
            postnummers.Add("<alles>");
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comPostNrs = conTuincentrum.CreateCommand())
                {
                    comPostNrs.CommandType = CommandType.Text;
                    comPostNrs.CommandText = "select postnr from leveranciers group by postnr order by postnr";

                    conTuincentrum.Open();

                    using (var rdrLeverancier = comPostNrs.ExecuteReader())
                    {
                        Int32 posPostNr = rdrLeverancier.GetOrdinal("PostNr");

                        while (rdrLeverancier.Read())
                        {
                            postnummers.Add(rdrLeverancier.GetString(posPostNr));
                        }
                    }
                }
            }
            return postnummers;
        }

        public void SchrijfVerwijderingen(List<Leverancier> leveranciers)
        {
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comDelete = conTuincentrum.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "delete from leveranciers where levnr=@levnr";

                    var parLevNr = comDelete.CreateParameter();
                    parLevNr.ParameterName = "@levnr";
                    comDelete.Parameters.Add(parLevNr);

                    conTuincentrum.Open();

                    foreach (Leverancier eenLeverancier in leveranciers)
                    {
                        parLevNr.Value = eenLeverancier.LevNr;
                        comDelete.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
