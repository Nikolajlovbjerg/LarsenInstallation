using Npgsql;

namespace Server.Repositories;

// Baseklasse som andre repositories arver fra
public abstract class BaseRepository
{
    // Connection string hentes fra konfiguration i stedet for at ligge i koden.
    // Lokalt: .NET user-secrets. I produktion: miljøvariabel (ConnectionStrings__Default).
    // Pooling=true er en del af selve connection stringen i konfigurationen.
    private readonly string _connectionString;

    protected BaseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "Connection string 'Default' mangler. Sæt ConnectionStrings:Default via user-secrets " +
                "eller miljøvariablen ConnectionStrings__Default.");
    }

    // Opretter en ny databaseforbindelse
    // Forbindelsen åbnes først, når den bruges
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
