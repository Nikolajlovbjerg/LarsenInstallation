using Server.PW1;
using Core;
using Npgsql;
using System;
using System.Xml.Linq;

namespace Server.Repositories.ProjCalc
{
    public class ProjectTotalCalcRepo : IProjectTotalCalcRepo
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";
    

    public ProjectTotalCalcRepo()
        {

        }

        public List<Project> GetAll()
        {
            var result = new List<Project>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"select * from projects";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ProjectId = reader.GetInt32(0);
                        var Name = reader.GetString(1);
                        var DateCreated = reader.GetDateTime(2);
                        var SvendTimePris = reader.GetInt32(3);
                        var LærlingTimePris = reader.GetInt32(4);
                        var KonsulentTimePris = reader.GetInt32(5);
                        var ArbjedsmandTimePris = reader.GetInt32(6);

                        Project p = new Project
                        { 
                            ProjectId = ProjectId,
                            Name = Name,
                            DateCreated = DateCreated,
                            SvendTimePris = SvendTimePris,
                            LærlingTimePris = LærlingTimePris,
                            KonsulentTimePris=KonsulentTimePris,
                            ArbjedsmandTimePris=ArbjedsmandTimePris
                        };
                        result.Add(p);
                    }
                }
            }
            return result;
        } 

    } 
}
