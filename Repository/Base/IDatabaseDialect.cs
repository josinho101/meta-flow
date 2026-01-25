using System.Data;

namespace Repository.Base
{
    public interface IDatabaseDialect
    {
        Task<IDbConnection> OpenConnectionAsync();
        Task<int> ExecuteNonQueryAsync(IDbConnection connection, string sql, Dictionary<string, object>? parameters = null);
        Task<object?> ExecuteScalarAsync(IDbConnection connection, string sql, Dictionary<string, object>? parameters = null);
        Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, string sql, Dictionary<string, object>? parameters = null);
        Task<bool> ExecuteTransactionAsync(List<Func<IDbConnection, IDbTransaction, Task>> operations);
    }
}
