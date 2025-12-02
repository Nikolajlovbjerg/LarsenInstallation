using Core;
using Npgsql;
using Server.PW1;
using Server.Repositories.ExcelRepos;

namespace Server.Repositories.User
{
    public class MaterialExcelRepo : IMaterialExcelRepo
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";

        public MaterialExcelRepo()
        {

        }
        public void Add(ProjectMaterial projmat)
        {
            var result = new List<ProjectMaterial>();


            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projectmaterials
                    (projectid, beskrivelse, kostpris, antal, total, avance, dækningsgrad) VALUES (@projectid, @beskrivelse, @kostpris, @antal, @total, @avance, @dækningsgrad)";

                Console.WriteLine(command.CommandText);
                var paramBeskriv = command.CreateParameter();
                paramBeskriv.ParameterName = "beskrivelse";
                command.Parameters.Add(paramBeskriv);
                paramBeskriv.Value = projmat.Beskrivelse;

                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "kostpris";
                command.Parameters.Add(paramKost);
                paramKost.Value = projmat.Kostpris;

                var paramAntal = command.CreateParameter();
                paramAntal.ParameterName = "antal";
                command.Parameters.Add(paramAntal);
                paramAntal.Value = projmat.Antal;

                var paramTotal = command.CreateParameter();
                paramTotal.ParameterName = "total";
                command.Parameters.Add(paramTotal);
                paramTotal.Value = projmat.Total;

                var PriceAvance = command.CreateParameter();
                PriceAvance.ParameterName = "avance";
                command.Parameters.Add(PriceAvance);
                PriceAvance.Value = projmat.Avance;

                var paramDaek = command.CreateParameter();
                paramDaek.ParameterName = "dækningsgrad";
                command.Parameters.Add(paramDaek);
                paramDaek.Value = projmat.Dækningsgrad;




                //skal fjernes på et tidspunkt
                var paramProjectId = command.CreateParameter();
                paramProjectId.ParameterName = "projectid";
                paramProjectId.Value = 1; // <-- hard coded for now
                command.Parameters.Add(paramProjectId);

                command.ExecuteNonQuery();
            }
        }
    }
}




