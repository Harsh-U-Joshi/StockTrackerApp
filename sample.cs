using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

class Program
{
    private const string ConnectionString = "";

    private const int MaxConcurrency = 8;      // Safe DB concurrency
    private static readonly SemaphoreSlim _semaphore =
        new(MaxConcurrency, MaxConcurrency);

    static async Task Main(string[] args)
    {
        string filePath = "user.txt";

        var stopwatch = Stopwatch.StartNew();

        await ProcessFileAsync(filePath);

        stopwatch.Stop();
        Console.WriteLine($"Total Time: {stopwatch.Elapsed}");
    }

    static async Task ProcessFileAsync(string filePath)
    {
        var runningTasks = new List<Task>(MaxConcurrency * 2);

        foreach (var line in File.ReadLines(filePath))
        {
            if (!long.TryParse(line.Trim(), out var userId))
                continue;

            var task = ExecuteSpAsync(userId);

            runningTasks.Add(task);

            // When we reach safe threshold, wait
            if (runningTasks.Count >= MaxConcurrency * 4)
            {
                await Task.WhenAll(runningTasks);
                runningTasks.Clear();
            }
        }

        if (runningTasks.Count > 0)
            await Task.WhenAll(runningTasks);
    }

    static async Task ExecuteSpAsync(long userId)
    {
        await _semaphore.WaitAsync();

        try
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("dbo.usp_ProcessUser", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@userId", SqlDbType.BigInt).Value = userId;

            await connection.OpenAsync();

            var sw = Stopwatch.StartNew();

            await command.ExecuteNonQueryAsync();

            sw.Stop();

            Console.WriteLine($"Processed {userId} in {sw.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR for userId {userId} : {ex.Message}");
            // Ideally log to Serilog file sink here
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
