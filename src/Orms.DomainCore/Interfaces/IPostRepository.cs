using Orms.Domain.DTOs;
using Orms.Domain.Entities;
using Orms.Domain.Entities.Pagination;

namespace Orms.Domain.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Create post
        /// </summary>
        /// <param name="postDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CreatePostUsersAsync(PostDto postDto, CancellationToken cancellationToken);

        /// <summary>
        /// Create respost
        /// </summary>
        /// <param name="postDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CreateRepostAsync(PostDtoRepost postDto, CancellationToken cancellationToken);

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post?> GetPostByIdAsync(int postId, CancellationToken cancellationToken);

        /// <summary>
        /// Get all posts
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetAllPostAsync(PaginationFilter filter,CancellationToken cancellationToken, int idUser=default);


        /// <summary>
        /// Return all posts by date
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="initial"></param>
        /// <param name="final"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetAllPostsByDateAsync(PaginationFilter filter, DateTime? initial, DateTime? final, CancellationToken cancellationToken);

    }
}
