using System.Data;

namespace Orms.Domain.Interfaces.DataBase
{
    /// <summary>
    /// Generic repository for write in database with Dapper
    /// </summary>
    public interface IApplicationWriteDbConnection : IApplicationReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object? param = default, IDbTransaction? transaction = default, CancellationToken cancellationToken = default);
    }
}
