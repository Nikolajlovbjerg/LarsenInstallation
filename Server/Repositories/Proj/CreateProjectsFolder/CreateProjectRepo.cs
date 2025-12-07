using Core;
using Npgsql;
using Server.PW1;

namespace Server.Repositories.Proj.CreateProjectsFolder
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
                command.CommandText = @"
                    INSERT INTO projects
                        (name, billedeurl ,datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbejdsmand_timepris) 
                    VALUES 
                        (@name, @billedeurl ,@datecreated, @svend_timepris, @lærling_timepris, @konsulent_timepris, @arbejdsmand_timepris)          
                        RETURNING projectid"; //Retunering query som sender projectid tilbage


                var paramStop = command.CreateParameter();
                paramStop.ParameterName = "name";
                command.Parameters.Add(paramStop);
                paramStop.Value = pro.Name;
                
                var paramBillede = command.CreateParameter();
                paramBillede.ParameterName = "billedeurl";
                command.Parameters.Add(paramBillede);
                paramBillede.Value = pro.ImageUrl;

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
                paramArb.Value = pro.ArbejdsmandTimePris;

                var newProjectId = (int)command.ExecuteScalar();
                return newProjectId; //retunere det nye id

                command.ExecuteNonQuery();
            }
        }

        public void AddHour(ProjectHour proj)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO projecthours
                        (projectid, medarbejder, dato, stoptid, timer, type, kostpris) 
                    VALUES 
                        (@projectid, @medarbejder, @dato, @stoptid, @timer, @type, @kostpris)";

                var paramProjId = command.CreateParameter();
                paramProjId.ParameterName = "projectid";
                command.Parameters.Add(paramProjId);
                paramProjId.Value = proj.ProjectId;
                
                var paramMed = command.CreateParameter();
                paramMed.ParameterName = "medarbejder";
                paramMed.Value = proj.Medarbejder ?? (object)DBNull.Value;
                command.Parameters.Add(paramMed);

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

                command.ExecuteNonQuery();
            }
        }
        
        
        
        public void AddMaterials(ProjectMaterial projmat)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO projectmaterials
                        (projectid, beskrivelse, kostpris, antal, leverandør, total, avance, dækningsgrad) 
                    VALUES 
                        (@projectid, @beskrivelse, @kostpris, @antal, @leverandør ,@total, @avance, @dækningsgrad)";


                var paramProjId = command.CreateParameter();
                paramProjId.ParameterName = "projectid";
                command.Parameters.Add(paramProjId);
                paramProjId.Value = projmat.ProjectId;

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
                
                var paramLev = command.CreateParameter();
                paramLev.ParameterName = "leverandør";
                command.Parameters.Add(paramLev);
                paramLev.Value = projmat.Leverandør;

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

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Project> GetAllProjects()
        {
            var projects = new List<Project>();

            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
        
                // Vi henter alle projekter, sorteret med nyeste først (valgfrit, men brugervenligt)
                string query = "SELECT * FROM projects " +
                               "ORDER BY datecreated " +
                               "DESC";

                using (var command = new NpgsqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Project
                        {
                            ProjectId = Convert.ToInt32(reader["projectid"]),
                            Name = reader["name"] == DBNull.Value ? "Ukendt" : reader["name"].ToString(),
                            DateCreated = reader["datecreated"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["datecreated"]),
                            ImageUrl = reader["billedeurl"] == DBNull.Value ? string.Empty : reader["billedeurl"].ToString(),
                            SvendTimePris = reader["svend_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["svend_timepris"]),
                            LærlingTimePris = reader["lærling_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["lærling_timepris"]),
                            KonsulentTimePris = reader["konsulent_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["konsulent_timepris"]),
                            ArbejdsmandTimePris = reader["arbejdsmand_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["arbejdsmand_timepris"])
                        };
                
                        projects.Add(p);
                    }
                }
            }

            return projects;
        }
       
        
        public Calculation? GetProjectDetails(int projectId)
        {
            using var conn = new NpgsqlConnection(conString);
            conn.Open();
            
            var dto = new Calculation();

            using (var command = new NpgsqlCommand("SELECT * FROM projects " +
                                                            "WHERE " +
                                                            "projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dto.Project = new Project
                    {
                        ProjectId = Convert.ToInt32(reader["projectid"]),
                        Name = reader["name"].ToString(),
                        ImageUrl = reader["billedeurl"] == DBNull.Value ? string.Empty : reader["billedeurl"].ToString(),
                        DateCreated = Convert.ToDateTime(reader["datecreated"]),
                        SvendTimePris = Convert.ToInt32(reader["svend_timepris"]),
                        LærlingTimePris = Convert.ToInt32(reader["lærling_timepris"]),
                        KonsulentTimePris = Convert.ToInt32(reader["konsulent_timepris"]),
                        ArbejdsmandTimePris = Convert.ToInt32(reader["arbejdsmand_timepris"])
                    };
                }
                else return null;
            }

            using (var command = new NpgsqlCommand("SELECT * FROM projectmaterials " +
                                                            "WHERE " +
                                                            "projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var m = new ProjectMaterial
                    {
                        ProjectId = Convert.ToInt32(reader["projectid"]),
                        Beskrivelse = reader["beskrivelse"] == DBNull.Value ? "" : reader["beskrivelse"].ToString(),
                        Kostpris = Convert.ToDecimal(reader["kostpris"]),
                        Antal = Convert.ToDecimal(reader["antal"]),
                        Total = Convert.ToDecimal(reader["total"]),
                        Leverandør = reader["leverandør"] == DBNull.Value ? "" : reader["leverandør"].ToString(),
                        Avance = Convert.ToDecimal(reader["avance"]),
                        Dækningsgrad = Convert.ToDecimal(reader["dækningsgrad"]),

                    };
                    dto.Materials.Add(m);

                    dto.TotalKostPrisMaterialer += m.Kostpris * m.Antal;
                    dto.TotalPrisMaterialer += m.Total;
                    
                }
            }


            using (var command = new NpgsqlCommand("SELECT * FROM projecthours " +
                                                            "WHERE " +
                                                            "projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dto.Hours.Add(new ProjectHour
                    {
                        ProjectId = Convert.ToInt32(reader["projectid"]),
                        // Henter navnet så vi kan koble overtid med rollen
                        Medarbejder = reader["medarbejder"] == DBNull.Value ? "Ukendt" : reader["medarbejder"].ToString(),
                        Dato = reader["dato"] == DBNull.Value ? null : Convert.ToDateTime(reader["dato"]),
                        Stoptid = reader["stoptid"] == DBNull.Value ? null : Convert.ToDateTime(reader["stoptid"]),
                        Timer = Convert.ToDecimal(reader["timer"]),
                        Type = reader["type"] == DBNull.Value ? "" : reader["type"].ToString(),
                        Kostpris = Convert.ToDecimal(reader["kostpris"]),
                    });
                }
            }

            // 4. BEREGN PRISER PÅ TIMER (Logik med LINQ)
            // Vi grupperer pr. medarbejder for at finde ud af, om de er Svend eller Lærling
            foreach (var group in dto.Hours.GroupBy(x => x.Medarbejder))
            {
                // A. Find rollen: Kig efter den første linje, der IKKE er overtid.
                var normalType = group
                    .FirstOrDefault(h => !h.Type.ToLower().Contains("overtid"))?
                    .Type.ToLower() ?? "svend"; // Fallback til Svend hvis intet findes

                // B. Find grundsatsen baseret på rollen
                decimal grundSats = 0;
                if (normalType.Contains("lærling"))       
                    grundSats = dto.Project.LærlingTimePris;
                else if (normalType.Contains("konsulent")) 
                    grundSats = dto.Project.KonsulentTimePris;
                else if (normalType.Contains("arbejdsmand")) 
                    grundSats = dto.Project.ArbejdsmandTimePris;
                else                                       
                    grundSats = dto.Project.SvendTimePris;

                // C. Gennemgå timerne og læg overtidstillæg på
                foreach (var h in group)
                {
                    decimal faktor = 1.0m;
                    string typeLower = h.Type.ToLower();

                    if (typeLower.Contains("overtid 1")) faktor = 1.5m; // 50% ekstra
                    else if (typeLower.Contains("overtid 2")) faktor = 2.0m; // 100% ekstra

                    // Beregn og gem
                    decimal salgsPrisForRække = h.Timer * grundSats * faktor;
                    
                    dto.TotalPrisTimer += salgsPrisForRække;
                    dto.TotalKostPrisTimer += h.Kostpris;
                }
            }

            // Opdater total timer til visning
            dto.TotalTimer = dto.Hours.Sum(h => h.Timer);
            
            return dto;
        }

        public void Update(Project p)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();

                // Her opdaterer vi navn, satser og billede hvor ID'et matcher
                command.CommandText = @"
                UPDATE projects SET 
                    name = @name, 
                    billedeurl = @billedeurl,
                    svend_timepris = @svend, 
                    lærling_timepris = @lærling, 
                    konsulent_timepris = @konsulent, 
                    arbejdsmand_timepris = @arbejdsmand
                WHERE projectid = @projectid";

                // Tilføj parametre
                command.Parameters.AddWithValue("projectid", p.ProjectId);
                command.Parameters.AddWithValue("name", p.Name ?? ""); // Sikrer at vi ikke sender null
                command.Parameters.AddWithValue("billedeurl", p.ImageUrl ?? "");
                command.Parameters.AddWithValue("svend", p.SvendTimePris);
                command.Parameters.AddWithValue("lærling", p.LærlingTimePris);
                command.Parameters.AddWithValue("konsulent", p.KonsulentTimePris);
                command.Parameters.AddWithValue("arbejdsmand", p.ArbejdsmandTimePris);

                command.ExecuteNonQuery();
            }

        }
        public void Delete(int id)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();

                command.CommandText = $"DELETE FROM projects WHERE projectid={id}";
                command.ExecuteNonQuery();
            }
        }

    }
}




