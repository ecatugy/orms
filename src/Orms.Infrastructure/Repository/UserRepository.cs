using Microsoft.EntityFrameworkCore;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces;
using Orms.Domain.Interfaces.DataBase;

namespace Orms.Persistence.Repository
{
    /// <summary>
    /// Repository of users
    /// </summary>
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(IApplicationDbContext dbContext, IApplicationReadDbConnection readDbConnection, IApplicationWriteDbConnection writeDbConnection) : base(dbContext, readDbConnection, writeDbConnection) { }


        /// <summary>
        /// Get user with their posts
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<User?> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            return DbContext.Users.Include(p => p.User_Post).FirstOrDefaultAsync(p => p.UserID == userId, cancellationToken);
        }


        /// <summary>
        /// Get all posts by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> CountPostsByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var sql = $@"SELECT COUNT(P.PostId) FROM Users U inner join Posts P on U.UserID=P.UserId where U.UserID={userId} and 
                        datediff(HOUR, P.DateInsert,GETDATE())  < 24";
            return ReadDbConnection.QuerySingleAsync<int>(sql, cancellationToken);

        }
    }
}
