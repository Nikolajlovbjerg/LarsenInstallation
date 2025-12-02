using Core;
using Npgsql;
using Server.PW1;
using Server.Repositories.ExcelRepos;

namespace Server.Repositories.User
{
    public class ExcelRepo : IExcelRepo
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";

        public ExcelRepo()
        {

        }
        public void Add(ProjectHour proj)
        {
            var result = new List<ProjectHour>();


            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projecthours
                    (projectid, dato, stoptid, timer, type, kostpris) VALUES (@projectid, @dato, @stoptid, @timer, @type, @kostpris)";

                Console.WriteLine(command.CommandText);
                var paramDato = command.CreateParameter();
                paramDato.ParameterName = "dato";
                command.Parameters.Add(paramDato);
                paramDato.Value = proj.Dato;

                var paramStop = command.CreateParameter();
                paramStop.ParameterName = "stoptid";
                command.Parameters.Add(paramStop);
                paramStop.Value = proj.Stoptid;

                var paramTimer = command.CreateParameter();
                paramTimer.ParameterName = "timer";
                command.Parameters.Add(paramTimer);
                paramTimer.Value = proj.Timer;

                var paramType = command.CreateParameter();
                paramType.ParameterName = "type";
                command.Parameters.Add(paramType);
                paramType.Value = proj.Type;

                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "kostpris";
                command.Parameters.Add(paramKost);
                paramKost.Value = proj.Kostpris;

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

        


