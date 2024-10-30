using Dapper;
using Microsoft.Data.SqlClient;

namespace StockTracker.API;


internal sealed class DatabaseInitializer(
    SqlConnection dataSource,
    IConfiguration configuration,
    ILogger<DatabaseInitializer> logger
   ) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Starting database initialization.");

            // await EnsureDatabaseExists();

            await InitializeDatabase();

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }

    private async Task EnsureDatabaseExists()
    {
        string connectionString = configuration.GetConnectionString("SqlConnectionString")!;

        var builder = new SqlConnectionStringBuilder(connectionString);

        string? databaseName = builder.InitialCatalog;

        builder.DataSource = "master"; // Connect to the default 'postgres' database

        using var connection = new SqlConnection(builder.ToString());

        await connection.OpenAsync();

        bool databaseExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM sys.databases WHERE datname = @databaseName)",
            new { databaseName });

        if (!databaseExists)
        {
            logger.LogInformation("Creating database {DatabaseName}", databaseName);

            await connection.ExecuteAsync($"CREATE DATABASE {databaseName}");
        }

        await connection.CloseAsync();
    }

    private async Task InitializeDatabase()
    {
        const string sql =
            """
            -- Check if the table exists, if not, create it
            CREATE TABLE EXISTS stock_prices (
                id INT IDENTITY(1,1) PRIMARY KEY,
                ticker NVARCHAR(10) NOT NULL,
                price DECIMAL(12, 6) NOT NULL,
                timestamp DATETIME DEFAULT (GETUTCDATE())
            );

            -- Create an index on the ticker column for faster lookups
            CREATE INDEX EXISTS idx_stock_prices_ticker ON stock_prices(ticker);

            -- Create an index on the timestamp column for faster time-based queries
            CREATE INDEX IF NOT EXISTS idx_stock_prices_timestamp ON stock_prices(timestamp);
            """;

        await dataSource.OpenAsync();

        await dataSource.ExecuteAsync(sql);

        await dataSource.CloseAsync();
    }
}