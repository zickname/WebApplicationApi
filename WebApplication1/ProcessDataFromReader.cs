using Npgsql;

internal delegate T ProcessDataFromReader<T>(NpgsqlDataReader reader);
public async Task<List<T>> ExecuteDatabaseOperationAsync<T>(string connectionString, string commandText, ProcessDataFromReader<T> processMethod, IDictionary<string, object> parameters = null)
{
    var results = new List<T>();

    using (var connection = new NpgsqlConnection(connectionString))
    {
        await connection.OpenAsync();

        using (var command = new NpgsqlCommand(commandText, connection))
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var result = processMethod(reader);
                    results.Add(result);
                }
            }
        }
    }

    return results;
}

