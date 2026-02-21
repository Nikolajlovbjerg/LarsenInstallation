using Npgsql;
using Server.PW1;

namespace Server.Repositories;

// Baseklasse som andre repositories arver fra
// Abstract class fungere som en skabelon (giver funktionalitet)
public abstract class BaseRepository
{
    // Connection string til online PostgreSQL-database (Neon)
    protected string ConnectionString =>
        "Server=ep-billowing-river-a2khj1xe.eu-central-1.aws.neon.tech;" + // Removed '-pooler'
        "Port=5432;" +
        "User Id=neondb_owner;" +
        "Password=" + PASSWORD.PW1 + ";" +
        "Database=Larsen_InstallationAps;" +
        "SSL Mode=Require;" +
        "Pooling=false;" +
        "Trust Server Certificate=true;";

    // Opretter en ny databaseforbindelse
    // Forbindelsen åbnes først, når den bruges
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}
