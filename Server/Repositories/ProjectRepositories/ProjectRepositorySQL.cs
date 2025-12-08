using Core;
using Npgsql;

namespace Server.Repositories.ProjectRepositories;

public class ProjectRepositorySQL : BaseRepository, IProjectRepositorySQL
{
    public int Create(Project pro)
    {
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projects
                (name, billedeurl, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbejdsmand_timepris) 
            VALUES 
                (@name, @billedeurl, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)          
            RETURNING projectid";

        command.Parameters.AddWithValue("name", pro.Name);
        command.Parameters.AddWithValue("billedeurl", pro.ImageUrl);
        command.Parameters.AddWithValue("datecreated", pro.DateCreated);
        command.Parameters.AddWithValue("svend", pro.SvendTimePris);
        command.Parameters.AddWithValue("lærling", pro.LærlingTimePris);
        command.Parameters.AddWithValue("konsulent", pro.KonsulentTimePris);
        command.Parameters.AddWithValue("arbejdsmand", pro.ArbejdsmandTimePris);

        return (int)command.ExecuteScalar()!;
    }

    public void Update(Project p)
    {
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = @"
            UPDATE projects SET 
                name = @name, 
                billedeurl = @billedeurl,
                svend_timepris = @svend, 
                lærling_timepris = @lærling, 
                konsulent_timepris = @konsulent, 
                arbejdsmand_timepris = @arbejdsmand
            WHERE projectid = @id";

        command.Parameters.AddWithValue("id", p.ProjectId);
        command.Parameters.AddWithValue("name", p.Name ?? "");
        command.Parameters.AddWithValue("billedeurl", p.ImageUrl ?? "");
        command.Parameters.AddWithValue("svend", p.SvendTimePris);
        command.Parameters.AddWithValue("lærling", p.LærlingTimePris);
        command.Parameters.AddWithValue("konsulent", p.KonsulentTimePris);
        command.Parameters.AddWithValue("arbejdsmand", p.ArbejdsmandTimePris);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "DELETE FROM projects WHERE projectid = @id";
        command.Parameters.AddWithValue("id", id);
        command.ExecuteNonQuery();
    }

    public Project? GetById(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projects WHERE projectid = @id";
        command.Parameters.AddWithValue("id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapProject(reader);
        }
        return null;
    }

    public IEnumerable<Project> GetAll()
    {
        var list = new List<Project>();
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projects ORDER BY datecreated DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(MapProject(reader));
        }
        return list;
    }

    // Hjælpemetode til at mappe fra DB til Objekt
    private Project MapProject(NpgsqlDataReader reader)
    {
        return new Project
        {
            ProjectId = Convert.ToInt32(reader["projectid"]),
            Name = reader["name"] == DBNull.Value ? "Ukendt" : reader["name"].ToString()!,
            DateCreated = reader["datecreated"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["datecreated"]),
            ImageUrl = reader["billedeurl"] == DBNull.Value ? string.Empty : reader["billedeurl"].ToString()!,
            SvendTimePris = reader["svend_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["svend_timepris"]),
            LærlingTimePris = reader["lærling_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["lærling_timepris"]),
            KonsulentTimePris = reader["konsulent_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["konsulent_timepris"]),
            ArbejdsmandTimePris = reader["arbejdsmand_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["arbejdsmand_timepris"])
        };
    }
}