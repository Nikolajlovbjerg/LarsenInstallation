using System;
using System.Xml.Linq;
using Core;
using Npgsql;
using Server.PW1;

namespace Server.Repositories.User
{
    public class CreateUserRepo : ICreateUserRepo
    {
        private const string conString =
        "Host=ep-spring-unit-a2y1k0pd-pooler.eu-central-1.aws.neon.tech;" +
        "Port=5432;" +
        "Username=neondb_owner;" +
        $"Password={PASSWORD.PW1};" +
        "Database=LarsensInstallation;" +
        "Ssl Mode=Require;" +
        "Trust Server Certificate=true;" +
        "Channel Binding=Require;";

        public CreateUserRepoPostgres()
        {

        }
        public List<User> GetAll()
        {
            var result = new List<User>();


            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM Users";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userid = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        var role = reader.GetString(3);

                        User u = new User
                        {
                            UserId = userid,
                            UserName = username,
                            Password = password,
                            Role = role
                        };
                        result.Add(u);
                    }
                }
            }

            return result.ToList();
        }

        public void Add(User user)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();

                command.CommandText =
                    "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";

                Console.WriteLine(command.CommandText);
                var paramName = command.CreateParameter();
                paramName.ParameterName = "username";
                command.Parameters.Add(paramName);
                paramName.Value = user.Username;

                var paramPassword = command.CreateParameter();
                paramPassword.ParameterName = "password";
                command.Parameters.Add(paramPassword);
                paramPassword.Value = user.Password;

                var paramRole = command.CreateParameter();
                paramRole.ParameterName = "role";
                command.Parameters.Add(paramRole);
                paramRole.Value = user.Description;

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();

                command.CommandText = $"DELETE FROM users WHERE userid={id}";
                command.ExecuteNonQuery();
            }
        }
    }
}


