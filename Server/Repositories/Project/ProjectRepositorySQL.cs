using Core;
using Npgsql;
using Server.PW1;

namespace Server.Repositories.Project
{
    public class ProjectRepositorySQL : IProjectRepository
    {
        private const string conString =
            "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
            "Port=5432;" +
            "Database=LarsenInstallation;" +
            "Username=neondb_owner;" +
            $"Password={PASSWORD.PW1};" + 
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;";

        // 1. OPRETTE PROJEKT (Denne har vi styr på, men den skal med)
        public int CreateProject(Core.Project project, List<ProjectHour> hours, List<ProjectMaterial> materials)
        {
            using var conn = new NpgsqlConnection(conString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Indsæt Projekt
                var cmdProject = new NpgsqlCommand(@"
                    INSERT INTO projects (name, svend_timepris, lærling_timepris, konsulent_timepris, arbjedsmand_timepris)
                    VALUES (@Name, @Svend, @Laerling, @Konsulent, @Arb)
                    RETURNING projectid", conn, transaction);

                cmdProject.Parameters.AddWithValue("Name", project.Name);
                cmdProject.Parameters.AddWithValue("Svend", project.SvendTimePris);
                cmdProject.Parameters.AddWithValue("Laerling", project.LærlingTimePris);
                cmdProject.Parameters.AddWithValue("Konsulent", project.KonsulentTimePris);
                cmdProject.Parameters.AddWithValue("Arb", project.ArbjedsmandTimePris);

                int newProjectId = (int)cmdProject.ExecuteScalar()!;

                // Indsæt Timer
                foreach (var h in hours)
                {
                    var cmdHour = new NpgsqlCommand(@"
                        INSERT INTO projecthours (projectid, dato, stoptid, timer, type, kostpris, raw_row)
                        VALUES (@Pid, @Dato, @Stop, @Timer, @Type, @Kost, @Raw)", conn, transaction);
                    
                    cmdHour.Parameters.AddWithValue("Pid", newProjectId);
                    cmdHour.Parameters.AddWithValue("Dato", h.Dato ?? (object)DBNull.Value);
                    cmdHour.Parameters.AddWithValue("Stop", h.Stoptid ?? (object)DBNull.Value);
                    cmdHour.Parameters.AddWithValue("Timer", h.Timer);
                    cmdHour.Parameters.AddWithValue("Type", h.Type ?? (object)DBNull.Value);
                    cmdHour.Parameters.AddWithValue("Kost", h.Kostpris);
                    cmdHour.Parameters.AddWithValue("Raw", h.RawRow ?? "");
                    cmdHour.ExecuteNonQuery();
                }

                // Indsæt Materialer
                foreach (var m in materials)
                {
                    var cmdMat = new NpgsqlCommand(@"
                        INSERT INTO projectmaterials (projectid, beskrivelse, antal, kostpris, total, avance, dækningsgrad, raw_row)
                        VALUES (@Pid, @Besk, @Antal, @Kost, @Total, @Avance, @Daek, @Raw)", conn, transaction);

                    cmdMat.Parameters.AddWithValue("Pid", newProjectId);
                    cmdMat.Parameters.AddWithValue("Besk", m.Beskrivelse ?? (object)DBNull.Value);
                    cmdMat.Parameters.AddWithValue("Antal", m.Antal);
                    cmdMat.Parameters.AddWithValue("Kost", m.Kostpris);
                    cmdMat.Parameters.AddWithValue("Total", m.Total);
                    cmdMat.Parameters.AddWithValue("Avance", m.Avance);
                    cmdMat.Parameters.AddWithValue("Daek", m.Dækningsgrad);
                    cmdMat.Parameters.AddWithValue("Raw", m.RawRow ?? "");
                    cmdMat.ExecuteNonQuery();
                }

                transaction.Commit();
                return newProjectId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // 2. HENT OG BEREGN DETALJER (Den nye del)
        public ProjectDetailsDTO? GetProjectDetails(int projectId)
        {
            using var conn = new NpgsqlConnection(conString);
            conn.Open();

            var dto = new ProjectDetailsDTO();

            // A. Hent Stamdata
            using (var cmd = new NpgsqlCommand("SELECT * FROM projects WHERE projectid = @Id", conn))
            {
                cmd.Parameters.AddWithValue("Id", projectId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dto.Project = new Core.Project
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

            // B. Hent Materialer og Beregn
            using (var cmd = new NpgsqlCommand("SELECT * FROM projectmaterials WHERE projectid = @Id", conn))
            {
                cmd.Parameters.AddWithValue("Id", projectId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var m = new ProjectMaterial
                    {
                        MaterialsId = reader.GetInt32(0),
                        Beskrivelse = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Kostpris = reader.GetDecimal(3),      // Enheds Kostpris
                        Antal = reader.GetDecimal(4),         // Antal
                        Total = reader.GetDecimal(5),         // Total Salgspris (fra Excel kolonne R)
                        Avance = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                        Dækningsgrad = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7)
                    };
                    dto.Materials.Add(m);

                    // BEREGNING MATERIALER:
                    // Kostpris = Enhedspris * Antal
                    dto.TotalKostprisMaterialer += (m.Kostpris * m.Antal);
                    
                    // Salgspris = Total kolonnen fra Excel
                    dto.TotalSalgsprisMaterialer += m.Total;
                }
            }

            // C. Hent Timer og Beregn
            using (var cmd = new NpgsqlCommand("SELECT * FROM projecthours WHERE projectid = @Id", conn))
            {
                cmd.Parameters.AddWithValue("Id", projectId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var h = new ProjectHour
                    {
                        HourId = reader.GetInt32(0),
                        Dato = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                        // Vi skipper Stoptid (index 3) i visningen hvis vi vil, men den er i DB
                        Timer = reader.GetDecimal(4),
                        Type = reader.IsDBNull(5) ? "" : reader.GetString(5), 
                        Kostpris = reader.GetDecimal(6) // Total kostpris for denne linje (fra Excel)
                    };
                    dto.Hours.Add(h);

                    // BEREGNING TIMER:
                    // Kostpris = Summen af kostpris kolonnerne
                    dto.TotalKostprisTimer += h.Kostpris;

                    // Salgspris = Timer * Sats (baseret på Type)
                    decimal timeSats = dto.Project.SvendTimePris; // Default
                    var typeL = h.Type.ToLower();

                    if (typeL.Contains("svend")) timeSats = dto.Project.SvendTimePris;
                    else if (typeL.Contains("lærling")) timeSats = dto.Project.LærlingTimePris;
                    else if (typeL.Contains("konsulent")) timeSats = dto.Project.KonsulentTimePris;
                    else if (typeL.Contains("arbejdsmand")) timeSats = dto.Project.ArbjedsmandTimePris;
                    
                    // Hvis det er "Overtid", bruger vi lige nu Svend satsen (medmindre du vil lave en Overtidssats)
                    
                    dto.TotalSalgsprisTimer += (h.Timer * timeSats);
                }
            }

            return dto;
        }
    }
}