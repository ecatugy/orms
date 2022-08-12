

using Dapper;
using Microsoft.Extensions.Configuration;
using Orms.Domain.Interfaces.DataBase;
using System.Data;
using System.Data.SqlClient;

namespace Orms.Persistence.Connections
{
    public class ApplicationReadDbConnection : IApplicationReadDbConnection, IDisposable
    {
        private readonly IDbConnection connection;

        public IDbConnection Connection => connection;

        public ApplicationReadDbConnection(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return connection.QueryAsync<T>(sql, param, transaction);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return connection.QuerySingleAsync<T>(sql, param, transaction);
        }

 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            connection.Dispose();   
        }
    }
}
