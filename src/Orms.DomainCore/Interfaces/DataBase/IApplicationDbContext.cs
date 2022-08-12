using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Orms.Domain.Entities;
using System.Data;

namespace Orms.Domain.Interfaces.DataBase
{
    /// <summary>
    /// Persistence Layer using Entity Framework Core
    /// </summary>
    public interface IApplicationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
