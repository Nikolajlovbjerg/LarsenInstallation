using Core;
using Npgsql;
using Server.PW1;
using Server.Repositories.ExcelRepos;

namespace Server.Repositories
{
    public class CreateProjectRepo : ICreateProjectRepo
    {
        private const string conString =
            "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
            "Port=5432;" +
            "Database=LarsenInstallation;" +
            "Username=neondb_owner;" +
            $"Password={PASSWORD.PW1};" +
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;";


        public int Add(Project pro) //Er en int fordi vi retunere et int
        {
            var result = new List<Project>();


            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projects
                    (name, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbejdsmand_timepris) 
                    VALUES (@name, @datecreated, @svend_timepris, @lærling_timepris, @konsulent_timepris, @arbejdsmand_timepris)          
                    RETURNING projectid"; //Retunering query som sender projectid tilbage


                var paramStop = command.CreateParameter();
                paramStop.ParameterName = "name";
                command.Parameters.Add(paramStop);
                paramStop.Value = pro.Name;

                var paramTimer = command.CreateParameter();
                paramTimer.ParameterName = "datecreated";
                command.Parameters.Add(paramTimer);
                paramTimer.Value = pro.DateCreated;

                var paramType = command.CreateParameter();
                paramType.ParameterName = "svend_timepris";
                command.Parameters.Add(paramType);
                paramType.Value = pro.SvendTimePris;

                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "lærling_timepris";
                command.Parameters.Add(paramKost);
                paramKost.Value = pro.LærlingTimePris;


                var paramKons = command.CreateParameter();
                paramKons.ParameterName = "konsulent_timepris";
                command.Parameters.Add(paramKons);
                paramKons.Value = pro.KonsulentTimePris;


                var paramArb = command.CreateParameter();
                paramArb.ParameterName = "arbejdsmand_timepris";
                command.Parameters.Add(paramArb);
                paramArb.Value = pro.ArbjedsmandTimePris;

                var newProjectId = (int)command.ExecuteScalar();
                return newProjectId; //retunere det nye id

                command.ExecuteNonQuery();
            }
        }


        public Calculation? GetProjectDetails(int projectId)
        {
            using var conn = new NpgsqlConnection(conString);
            conn.Open();
            
            var dto = new Calculation();

            using (var command = new NpgsqlCommand("SELECT * FROM projects WHERE projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dto.project = new Project
                    {
                        ProjectId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        DateCreated = reader.GetDateTime(2),
                        SvendTimePris = reader.GetInt32(3),
                        LærlingTimePris = reader.GetInt32(4),
                        KonsulentTimePris = reader.GetInt32(5),
                        ArbjedsmandTimePris = reader.GetInt32(6)
                    };
                }
                else return null;
            }

            using (var command = new NpgsqlCommand("SELECT * FROM projectmaterials WHERE projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var m = new ProjectMaterial
                    {
                        ProjectId = reader.GetInt32(0),
                        MaterialsId = reader.GetInt32(1),
                        Beskrivelse = reader.GetString(2),
                        Kostpris = reader.GetDecimal(3),
                        Antal = reader.GetDecimal(4),
                        Total = reader.GetDecimal(5),
                        Avance = reader.GetDecimal(6),
                        Dækningsgrad = reader.GetDecimal(7),

                    };
                    dto.Materials.Add(m);

                    dto.TotalKostPrisMaterialer += (m.Kostpris * m.Antal);
                    dto.TotalPrisMaterialer += m.Total;
                    
                }
            }


            using (var command = new NpgsqlCommand("SELECT * FROM projecthours WHERE projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var h = new ProjectHour
                    {
                        ProjectId = reader.GetInt32(0),
                        HourId = reader.GetInt32(1),
                        Dato = reader.GetDateTime(2),
                        Timer = reader.GetDecimal(3),
                        Type = reader.GetString(4),
                        Kostpris = reader.GetDecimal(5),
                    };
                    dto.Hours.Add(h);
                    
                    dto.TotalKostPrisTimer += h.Kostpris;

                    decimal timeSats = dto.project.SvendTimePris;
                    var type = h.Type.ToLower();

                    if (type.Contains("svend")) timeSats = dto.project.SvendTimePris;
                    else if (type.Contains("lærling")) timeSats = dto.project.LærlingTimePris;
                    else if (type.Contains("konsulent")) timeSats = dto.project.KonsulentTimePris;
                    else if (type.Contains("arbejdsmand")) timeSats = dto.project.ArbjedsmandTimePris;
                    
                    dto.TotalPrisTimer += (h.Timer * timeSats);

                }
            }
            
            return dto;
        }

    }
}




