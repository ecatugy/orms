using Orms.Domain.Entities;

namespace Orms.Domain.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get user with their posts
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<User?> GetUserAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Get all posts by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountPostsByUserAsync(int userId, CancellationToken cancellationToken);

    }
}
