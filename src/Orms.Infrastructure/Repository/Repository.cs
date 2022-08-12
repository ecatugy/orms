using Orms.Domain.Interfaces;
using Orms.Domain.Interfaces.DataBase;

namespace Orms.Persistence.Repository
{
    /// <summary>
    /// Connections of repositories with Dapper and Entity Framework
    /// </summary>
    public class Repository : IRepository
    {
        public IApplicationDbContext DbContext { get; }

        public IApplicationReadDbConnection ReadDbConnection { get; }

        public IApplicationWriteDbConnection WriteDbConnection { get; }

        public Repository(IApplicationDbContext dbContext,
            IApplicationReadDbConnection readDbConnection,
            IApplicationWriteDbConnection writeDbConnection)
        {

            DbContext = dbContext;
            ReadDbConnection = readDbConnection;
            WriteDbConnection = writeDbConnection;  
        }
    }
}
