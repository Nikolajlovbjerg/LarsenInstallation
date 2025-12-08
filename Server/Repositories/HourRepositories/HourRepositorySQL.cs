using Core;
using Npgsql;

namespace Server.Repositories.HourRepositories;

public class HourRepositorySQL : BaseRepository, IHourRepositorySQL
{
    public void Add(ProjectHour h)
    {
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projecthours
                (projectid, medarbejder, dato, stoptid, timer, type, kostpris) 
            VALUES 
                (@pid, @med, @dato, @stop, @timer, @type, @kost)";

        command.Parameters.AddWithValue("pid", h.ProjectId);
        command.Parameters.AddWithValue("med", h.Medarbejder ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("dato", h.Dato ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("stop", h.Stoptid ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("timer", h.Timer);
        command.Parameters.AddWithValue("type", h.Type ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("kost", h.Kostpris);

        command.ExecuteNonQuery();
    }

    public List<ProjectHour> GetByProjectId(int projectId)
    {
        var list = new List<ProjectHour>();
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projecthours WHERE projectid = @id";
        command.Parameters.AddWithValue("id", projectId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new ProjectHour
            {
                HourId = Convert.ToInt32(reader["hourid"] is DBNull ? 0 : reader["hourid"]), // Håndter hvis ID kolonne mangler eller hedder noget andet
                ProjectId = Convert.ToInt32(reader["projectid"]),
                Medarbejder = reader["medarbejder"] == DBNull.Value ? "Ukendt" : reader["medarbejder"].ToString(),
                Dato = reader["dato"] == DBNull.Value ? null : Convert.ToDateTime(reader["dato"]),
                Stoptid = reader["stoptid"] == DBNull.Value ? null : Convert.ToDateTime(reader["stoptid"]),
                Timer = Convert.ToDecimal(reader["timer"]),
                Type = reader["type"] == DBNull.Value ? "" : reader["type"].ToString(),
                Kostpris = Convert.ToDecimal(reader["kostpris"]),
            });
        }
        return list;
    }
    
    public List<Calculation> GetTotalHoursGroupedByType()
    {
        var result = new List<Calculation>();
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        
        command.CommandText = @"
            SELECT projectid, type, SUM(timer) AS total_hours
            FROM projecthours
            GROUP BY projectid, type
            ORDER BY projectid, type";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Calculation
            {
                ProjectId = reader.GetInt32(0),
                Type = reader.GetString(1),
                TotalHours = reader.GetDecimal(2)
            });
        }
        return result;
    }
}