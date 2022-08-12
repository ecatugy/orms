using Dapper;
using Orms.Domain.Interfaces.DataBase;
using System.Data;

namespace Orms.Persistence.Connections
{
    public class ApplicationWriteDbConnection : IApplicationWriteDbConnection
    {
        private readonly IApplicationDbContext context;

        public ApplicationWriteDbConnection(IApplicationDbContext context)
        {
            this.context = context;
        }

        public IDbConnection Connection => throw new NotImplementedException();

        public Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return context.Connection.ExecuteAsync(sql, param, transaction);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return context.Connection.QueryAsync<T>(sql, param, transaction);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return context.Connection.QuerySingleAsync<T>(sql, param, transaction);
        }
    }
}
