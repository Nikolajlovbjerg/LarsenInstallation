using Core;
using Npgsql;

namespace Server.Repositories.MaterialRepositories;

public class MaterialRepositorySQL : BaseRepository, IMaterialRepositorySQL
{
    public void Add(ProjectMaterial m)
    {
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projectmaterials
                (projectid, beskrivelse, kostpris, antal, leverandør, total, avance, dækningsgrad) 
            VALUES 
                (@pid, @besk, @kost, @antal, @lev, @total, @avance, @dg)";

        command.Parameters.AddWithValue("pid", m.ProjectId);
        command.Parameters.AddWithValue("besk", m.Beskrivelse ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("kost", m.Kostpris);
        command.Parameters.AddWithValue("antal", m.Antal);
        command.Parameters.AddWithValue("lev", m.Leverandør ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("total", m.Total);
        command.Parameters.AddWithValue("avance", m.Avance);
        command.Parameters.AddWithValue("dg", m.Dækningsgrad);

        command.ExecuteNonQuery();
    }

    public List<ProjectMaterial> GetByProjectId(int projectId)
    {
        var list = new List<ProjectMaterial>();
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projectmaterials WHERE projectid = @id";
        command.Parameters.AddWithValue("id", projectId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new ProjectMaterial
            {
                MaterialsId = Convert.ToInt32(reader["materialsid"] is DBNull ? 0 : reader["materialsid"]),
                ProjectId = Convert.ToInt32(reader["projectid"]),
                Beskrivelse = reader["beskrivelse"] == DBNull.Value ? "" : reader["beskrivelse"].ToString(),
                Kostpris = Convert.ToDecimal(reader["kostpris"]),
                Antal = Convert.ToDecimal(reader["antal"]),
                Total = Convert.ToDecimal(reader["total"]),
                Leverandør = reader["leverandør"] == DBNull.Value ? "" : reader["leverandør"].ToString(),
                Avance = Convert.ToDecimal(reader["avance"]),
                Dækningsgrad = Convert.ToDecimal(reader["dækningsgrad"]),
            });
        }
        return list;
    }
}