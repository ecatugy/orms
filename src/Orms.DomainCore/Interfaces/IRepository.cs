using Orms.Domain.Interfaces.DataBase;

namespace Orms.Domain.Interfaces
{
    /// <summary>
    /// Interface de repositório
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Entity Framework
        /// </summary>
        IApplicationDbContext DbContext { get; }

        /// <summary>
        /// Read Dapper
        /// </summary>
        IApplicationReadDbConnection ReadDbConnection { get; }

        /// <summary>
        /// Write Dapper
        /// </summary>
        IApplicationWriteDbConnection WriteDbConnection { get; }
    }
}
