using Npgsql;
using Server.PW1;

namespace Server.Repositories;

// Baseklasse som andre repositories arver fra
public abstract class BaseRepository
{
    // Connection string til online PostgreSQL-database
    protected string ConnectionString =>
        "Server=ep-billowing-river-a2khj1xe.eu-central-1.aws.neon.tech;" + 
        "Port=5432;" +
        "User Id=neondb_owner;" +
        "Password=" + PASSWORD.PW1 + ";" +
        "Database=Larsen_InstallationAps;" +
        "SSL Mode=Require;" +
        "Pooling=true;" +
        "Trust Server Certificate=true;";

    // Opretter en ny databaseforbindelse
    // Forbindelsen åbnes først, når den bruges
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}
