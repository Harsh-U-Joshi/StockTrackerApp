static IEnumerable<List<long>> GetBatches(string filePath, int batchSize)
{
    var batch = new List<long>(batchSize);

    foreach (var line in File.ReadLines(filePath))
    {
        if (long.TryParse(line, out var id))
        {
            batch.Add(id);

            if (batch.Count >= batchSize)
            {
                yield return batch;
                batch = new List<long>(batchSize);
            }
        }
    }

    if (batch.Count > 0)
        yield return batch;
}

static async Task ProcessAsync(string filePath)
{
    var batches = GetBatches(filePath, 10000);

    var semaphore = new SemaphoreSlim(8); // MAX 8 parallel calls

    var tasks = batches.Select(async batch =>
    {
        await semaphore.WaitAsync();
        try
        {
            await ExecuteBatchAsync(batch);
        }
        finally
        {
            semaphore.Release();
        }
    });

    await Task.WhenAll(tasks);
}

static async Task ExecuteBatchAsync(List<long> memberIds)
{
    using var connection = new SqlConnection("your_connection");
    using var command = new SqlCommand("dbo.usp_ProcessMembers", connection);

    command.CommandType = CommandType.StoredProcedure;

    var table = new DataTable();
    table.Columns.Add("MemberId", typeof(long));

    foreach (var id in memberIds)
        table.Rows.Add(id);

    var param = command.Parameters.AddWithValue("@MemberIds", table);
    param.SqlDbType = SqlDbType.Structured;
    param.TypeName = "dbo.MemberIdList";

    await connection.OpenAsync();
    await command.ExecuteNonQueryAsync();
}
