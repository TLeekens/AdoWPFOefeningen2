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

        public void SchrijfToevoegingen(List<Leverancier> leveranciers)
        {
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comInsert = conTuincentrum.CreateCommand())
                {
                    comInsert.CommandType = CommandType.Text;
                    comInsert.CommandText = "insert into leveranciers (naam, adres, postnr, woonplaats) values (@naam, @adres, @postnr, @woonplaats)";

                    var parNaam = comInsert.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comInsert.Parameters.Add(parNaam);

                    var parAdres = comInsert.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comInsert.Parameters.Add(parAdres);

                    var parPostNr = comInsert.CreateParameter();
                    parPostNr.ParameterName = "@postnr";
                    comInsert.Parameters.Add(parPostNr);

                    var parWoonplaats = comInsert.CreateParameter();
                    parWoonplaats.ParameterName = "@woonplaats";
                    comInsert.Parameters.Add(parWoonplaats);

                    conTuincentrum.Open();

                    foreach (Leverancier lev in leveranciers)
                    {
                        parNaam.Value = lev.Naam;
                        parAdres.Value = lev.Adres;
                        parPostNr.Value = lev.PostNr;
                        parWoonplaats.Value = lev.Woonplaats;

                        comInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        public void SchrijfWijzigingen(List<Leverancier> leveranciers)
        {
            var manager = new TuincentrumDbManager();

            using (var conTuincentrum = manager.GetConnection())
            {
                using (var comUpdate = conTuincentrum.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update leveranciers set naam=@naam, adres=@adres, postnr=@postnr, woonplaats=@woonplaats where levnr=@levnr";

                    var parNaam = comUpdate.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comUpdate.Parameters.Add(parNaam);

                    var parAdres = comUpdate.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comUpdate.Parameters.Add(parAdres);

                    var parPostNr = comUpdate.CreateParameter();
                    parPostNr.ParameterName = "@postnr";
                    comUpdate.Parameters.Add(parPostNr);

                    var parWoonplaats = comUpdate.CreateParameter();
                    parWoonplaats.ParameterName = "@woonplaats";
                    comUpdate.Parameters.Add(parWoonplaats);

                    var parLevNr = comUpdate.CreateParameter();
                    parLevNr.ParameterName = "@levnr";
                    comUpdate.Parameters.Add(parLevNr);

                    conTuincentrum.Open();

                    foreach (Leverancier lev in leveranciers)
                    {
                        parNaam.Value = lev.Naam;
                        parAdres.Value = lev.Adres;
                        parPostNr.Value = lev.PostNr;
                        parWoonplaats.Value = lev.Woonplaats;
                        parLevNr.Value = lev.LevNr;

                        comUpdate.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
