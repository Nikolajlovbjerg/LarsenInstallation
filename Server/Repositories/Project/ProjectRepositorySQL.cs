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

        public int CreateProject(Core.Project project, List<ProjectHour> hours, List<ProjectMaterial> materials)
        {
            using var conn = new NpgsqlConnection(conString);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. INDSÆT PROJEKT
                // Rettet: Bruger nu snake_case kolonnenavne (fx svend_timepris)
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

                // 2. INDSÆT TIMER
                // Bemærk: Din tabel 'projecthours' har ikke 'medarbejder' eller 'beskrivelse' kolonner.
                // Jeg gemmer derfor kun de felter, der findes i tabellen (dato, timer, type, raw_row).
                // Hvis du vil gemme beskrivelse, kan vi sætte det ind i 'type' eller 'raw_row'.
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

                // 3. INDSÆT MATERIALER
                // Rettet: Din tabel har ikke 'varenummer', så den er fjernet.
                // Tilføjet: avance og dækningsgrad
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
    }
}