using Npgsql;
using Repository.Base;
using System.Data;

namespace Repository.Postgres.Admin
{
    public abstract class PostgresDialect : IDatabaseDialect
    {
        private readonly string connectionString;

        public PostgresDialect(string connectionString)
        {
            this.connectionString = connectionString;
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

        public async Task<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction trans, string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new NpgsqlCommand(sql, (NpgsqlConnection?)connection, (NpgsqlTransaction?)trans);
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
