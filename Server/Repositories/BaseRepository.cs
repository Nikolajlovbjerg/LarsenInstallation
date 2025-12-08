using Npgsql;
using Server.PW1;

namespace Server.Repositories;

public abstract class BaseRepository
{
    protected string ConnectionString => 
        "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
        "Port=5432;" +
        "Database=LarsenInstallation;" +
        "Username=neondb_owner;" +
        $"Password={PASSWORD.PW1};" +
        "Ssl Mode=Require;" +
        "Trust Server Certificate=true;";

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}