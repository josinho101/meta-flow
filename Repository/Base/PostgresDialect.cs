using Npgsql;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Repository.Base
{
    public class PostgresDialect : IDatabaseDialect
    {
        private readonly string connectionString;

        public PostgresDialect(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(this.connectionString))
            {
                throw new Exception("Connection string 'DefaultConnection' is not found or is empty.");
            }
        }

        public async Task<IDbConnection> OpenConnectionAsync()
        {
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task CloseConnectionAsync(IDbConnection connection)
        {
            if (connection is NpgsqlConnection sqlConnection)
            {
                await sqlConnection.CloseAsync();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(IDbConnection connection, string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection?)connection);
            AddParameters(cmd, parameters);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, string sql, Dictionary<string, object>? parameters = null)
        {
            var cmd = new NpgsqlCommand(sql, (NpgsqlConnection?)connection);
            AddParameters(cmd, parameters);
            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public async Task<object?> ExecuteScalarAsync(IDbConnection connection, string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection?)connection);
            AddParameters(cmd, parameters);
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<bool> ExecuteTransactionAsync(List<Func<IDbConnection, IDbTransaction, Task>> operations)
        {
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                foreach (var operation in operations)
                    await operation(conn, transaction);

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private void AddParameters(NpgsqlCommand cmd, Dictionary<string, object>? parameters)
        {
            if (parameters == null) return;
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }
    }
}
