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
                var paramBrand = command.CreateParameter();
                paramBrand.ParameterName = "dato";
                command.Parameters.Add(paramBrand);
                paramBrand.Value = proj.Dato;

                var paramModel = command.CreateParameter();
                paramModel.ParameterName = "stoptid";
                command.Parameters.Add(paramModel);
                paramModel.Value = proj.Stoptid;

                var paramDesc = command.CreateParameter();
                paramDesc.ParameterName = "timer";
                command.Parameters.Add(paramDesc);
                paramDesc.Value = proj.Timer;

                var paramPrice = command.CreateParameter();
                paramPrice.ParameterName = "type";
                command.Parameters.Add(paramPrice);
                paramPrice.Value = proj.Type;

                var paramImg = command.CreateParameter();
                paramImg.ParameterName = "kostpris";
                command.Parameters.Add(paramImg);
                paramImg.Value = proj.Kostpris;

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

        


