using Server.PW1;
using Npgsql;
using Core;

namespace Server.Repositories.Proj;

public class ProjectRepositorySQL : IProjectRepo
{
    private const string conString =
        "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
        "Port=5432;" +
        "Database=LarsenInstallation;" +
        "Username=neondb_owner;" +
        $"Password={PASSWORD.PW1};" +
        "Ssl Mode=Require;" +
        "Trust Server Certificate=true;";


    public List<Project> GetAll()
    {
        var result = new List<Project>();
        
        using (var mConnection = new NpgsqlConnection(conString))
        {
            mConnection.Open();
            var command = mConnection.CreateCommand();
            command.CommandText = @"SELECT * FROM projects";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var projectid = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var datecreated = reader.GetDateTime(2);
                    var svendtimepris = reader.GetInt32(3);
                    var lærlingtimepris = reader.GetInt32(4);
                    var konsulenttimepris = reader.GetInt32(5);
                    var arbejdsmandtimepris = reader.GetInt32(6);


                    Project p = new Project
                    {
                        ProjectId = projectid,
                        Name = name,
                        DateCreated = datecreated,
                        SvendTimePris = svendtimepris,
                        LærlingTimePris = lærlingtimepris,
                        KonsulentTimePris = konsulenttimepris,
                        ArbejdsmandTimePris = arbejdsmandtimepris
                    };
                    result.Add(p);
                }
            }
        }

        return result;
    }
}