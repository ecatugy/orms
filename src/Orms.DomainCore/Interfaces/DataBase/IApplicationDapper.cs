using System.Data;

namespace Orms.Domain.Interfaces.DataBase
{
    /// <summary>
    /// Connection for use dapper
    /// </summary>
    public interface IApplicationDapper
    {
        public IDbConnection Connection { get; }
        
    }
    
}

