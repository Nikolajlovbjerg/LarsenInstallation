using System.Data.Common;
using Core;                   

namespace Server.Repositories.User
{
    public class CreateUserRepoSQL : BaseRepository, ICreateUserRepo
    {
    public List<Users> GetAll()
        {

            var result = new List<Users>();    // Tom liste til brugere der hentes

            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection(); //Bruger using for at sikre at forbindelsen bliver lukket(Using = sørger for at det bliver lukket)
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand();   // Opretter SQL-kommando
                command.CommandText = @"SELECT * FROM Users"; // SQL der henter alle brugere
                
                using (var reader = command.ExecuteReader())  // Kører SELECT og får en "reader" (Læser kolloner) 
                {
                    while (reader.Read()) // Læser én række ad gangen
                    {
                        // Henter værdier fra kolonnerne i rækken
                        var userid = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        var role = reader.GetString(3);

                        // Mapper database-værdier til et Users-objekt (lægger data ind i C# objekt)
                        Users u = new Users
                        {
                            UserId = userid,
                            UserName = username,
                            Password = password,
                            Role = role
                        };
                        result.Add(u); // Tilføjer bruger til listen
                    }
                }
            }

            return result; // Returnerer hele listen
        }

        public void Add(Users user)
        {
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection();
            mConnection.Open(); // Åbner forbindelsen
            {

                var command = mConnection.CreateCommand(); // Ny SQL-kommando

                // SQL med parametre
                command.CommandText =
                    "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";

                // Opretter parameter for @username
                var paramName = command.CreateParameter();
                paramName.ParameterName = "username"; // Navn der matcher @username
                command.Parameters.Add(paramName); // Tilføjer til SQL-kommandos parameterliste
                paramName.Value = user.UserName; // Sætter værdien

                // Opretter parameter for @password
                var paramPassword = command.CreateParameter();
                paramPassword.ParameterName = "password";
                command.Parameters.Add(paramPassword);
                paramPassword.Value = user.Password;

                // Opretter parameter for @role
                var paramRole = command.CreateParameter();
                paramRole.ParameterName = "role";
                command.Parameters.Add(paramRole);
                paramRole.Value = user.Role;

                command.ExecuteNonQuery();   // Eksekverer INSERT (kører SQL-kommandoen)
            }
        }

        public Users? ValidateUser(string username, string password)
        {
            // Henter kun den ene bruger der matcher brugernavnet (i stedet for at hente alle)
            using var mConnection = GetConnection();
            mConnection.Open();

            var command = mConnection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE username = @username";

            var paramName = command.CreateParameter();
            paramName.ParameterName = "username";
            paramName.Value = username;
            command.Parameters.Add(paramName);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null; // Ingen bruger med det brugernavn

            var user = new Users
            {
                UserId = reader.GetInt32(0),
                UserName = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };

            // Sammenligner password (bør hashes senere)
            return user.Password == password ? user : null;
        }

        public void Delete(int id)
        {
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection();
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand(); // Ny SQL-kommando

                command.CommandText = "DELETE FROM users WHERE userid = @id"; // SQL der sletter ud fra id

                // Sender id som parameter (undgår SQL injection)
                var paramId = command.CreateParameter();
                paramId.ParameterName = "id";
                paramId.Value = id;
                command.Parameters.Add(paramId);

                command.ExecuteNonQuery();   // Kører DELETE-kommandoen
            }
        }
        
        public Users? GetById(int id)
        {
            using var mConnection = GetConnection();
            mConnection.Open();
            var command = mConnection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE userid = @id";

            var paramId = command.CreateParameter();
            paramId.ParameterName = "id";
            paramId.Value = id;
            command.Parameters.Add(paramId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Users
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = reader.GetString(3)
                };
            }
            return null;
        }

        public void Update(Users user)
        {
            using var mConnection = GetConnection();
            mConnection.Open();
            var command = mConnection.CreateCommand();
    
            command.CommandText = @"UPDATE Users 
                            SET username = @username, 
                                password = @password, 
                                role = @role 
                            WHERE userid = @id";

            // Tilføj parametre (vigtigt for at undgå SQL injection)
            var pId = command.CreateParameter(); pId.ParameterName = "id"; pId.Value = user.UserId;
            command.Parameters.Add(pId);

            var pName = command.CreateParameter(); pName.ParameterName = "username"; pName.Value = user.UserName;
            command.Parameters.Add(pName);

            var pPass = command.CreateParameter(); pPass.ParameterName = "password"; pPass.Value = user.Password;
            command.Parameters.Add(pPass);

            var pRole = command.CreateParameter(); pRole.ParameterName = "role"; pRole.Value = user.Role;
            command.Parameters.Add(pRole);

            command.ExecuteNonQuery();
        }
    }
}
