using System.Data;

namespace Orms.Domain.Interfaces.DataBase
{
    /// <summary>
    /// Generic repository for work with Dapper
    /// </summary>
    public interface IApplicationReadDbConnection:IApplicationDapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = default, IDbTransaction? transaction = default, CancellationToken cancellationToken = default);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = default, IDbTransaction? transaction = default, CancellationToken cancellationToken = default);

        Task<T> QuerySingleAsync<T>(string sql, object? param = default, IDbTransaction? transaction = default, CancellationToken cancellationToken = default);
    }
}
