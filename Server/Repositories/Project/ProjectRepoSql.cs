// Server/Repositories/Project/ProjectRepoSQL.cs
using Core;
using Npgsql;
using Server.PW1;
using System;
using System.Collections.Generic;
using System.Linq;
/*
namespace Server.Repositories.Project
{
    public class ProjectRepoSQL : IProjectRepo
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

            using var mConnection = new NpgsqlConnection(conString);
            mConnection.Open();
            using var command = mConnection.CreateCommand();
            command.CommandText = "SELECT projectid, name, datecreated, svendtimepris, lærlingtimepris, konsulenttimepris, arbejdsmandtimepris FROM Projects";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var p = new Project
                {
                    ProjectId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    DateCreated = reader.GetDateTime(2),
                    SvendTimePris = reader.GetDecimal(3),
                    LærlingTimePris = reader.GetDecimal(4),
                    KonsulentTimePris = reader.GetDecimal(5),
                    ArbejdsmandTimePris = reader.GetDecimal(6),
                    Houses = new List<House>(),      // Placeholder – kan udbygges senere
                    TimeEntries = new List<TimeEntry>()
                };
                result.Add(p);
            }

            return result;
        }

        public void Add(Project p)
        {
            using var mConnection = new NpgsqlConnection(conString);
            mConnection.Open();
            using var command = mConnection.CreateCommand();

            command.CommandText =
                "INSERT INTO Projects (name, datecreated, svendtimepris, lærlingtimepris, konsulenttimepris, arbejdsmandtimepris) " +
                "VALUES (@name, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)";

            command.Parameters.AddWithValue("name", p.Name);
            command.Parameters.AddWithValue("datecreated", p.DateCreated);
            command.Parameters.AddWithValue("svend", p.SvendTimePris);
            command.Parameters.AddWithValue("lærling", p.LærlingTimePris);
            command.Parameters.AddWithValue("konsulent", p.KonsulentTimePris);
            command.Parameters.AddWithValue("arbejdsmand", p.ArbejdsmandTimePris);

            command.ExecuteNonQuery();
        }

        public void DeleteById(int id)
        {
            using var mConnection = new NpgsqlConnection(conString);
            mConnection.Open();
            using var command = mConnection.CreateCommand();

            command.CommandText = "DELETE FROM Projects WHERE projectid=@id";
            command.Parameters.AddWithValue("id", id);

            command.ExecuteNonQuery();
        }

        public Project? GetById(int id)
        {
            return GetAll().FirstOrDefault(p => p.ProjectId == id);
        }
    }
}
*/