using Microsoft.EntityFrameworkCore;
using Orms.Domain.DTOs;
using Orms.Domain.Entities;
using Orms.Domain.Entities.Pagination;
using Orms.Domain.Enuns;
using Orms.Domain.Interfaces;
using Orms.Domain.Interfaces.DataBase;
using System.Data.Common;
namespace Orms.Persistence.Repository
{
    /// <summary>
    /// Repositories of posts
    /// </summary>
    public class PostRepository : Repository, IPostRepository
    {
        public PostRepository(IApplicationDbContext dbContext, IApplicationReadDbConnection readDbConnection, IApplicationWriteDbConnection writeDbConnection) : base(dbContext, readDbConnection, writeDbConnection) { }

        public async Task<int> CreatePostUsersAsync(PostDto postDto, CancellationToken cancellationToken)
        {
            return await CreatePostAsync(new Post
            {
                Type = postDto.Type,
                Content = postDto.Content,
                UserId = postDto.UserId,
                DateInsert = DateTime.Now,
            }, cancellationToken);
        }


        public async Task<int> CreateRepostAsync(PostDtoRepost postDto, CancellationToken cancellationToken)
        {
            return await CreatePostByTypeAsync(postDto, PostType.Repost, cancellationToken);
        }

 
        private async Task<int> CreatePostByTypeAsync(PostDtoRepost postDto, PostType type, CancellationToken cancellationToken)
        {
            return await CreatePostAsync(new Post
            {
                Type = type,
                Content = postDto.Content,
                UserId = postDto.UserId,
                DateInsert = DateTime.Now,
                Comment = postDto.Comment
            }, cancellationToken);
        }


        /// <summary>
        /// Create the post
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> CreatePostAsync(Post post, CancellationToken cancellationToken)
        {
            DbContext.Connection.Open();
            using var transaction = DbContext.Connection.BeginTransaction();

            try
            {
                DbContext.Database.UseTransaction(transaction as DbTransaction);
                //Add Post
                await DbContext.Posts.AddAsync(post, cancellationToken);
                await DbContext.SaveChangesAsync(cancellationToken);

                //Commmit
                transaction.Commit();

                //Return PostId
                return post.PostId;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                DbContext.Connection.Close();
            }
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Post?> GetPostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            return DbContext.Posts.FirstOrDefaultAsync(p => p.PostId == postId, cancellationToken);
        }

        /// <summary>
        /// Return all posts
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetAllPostAsync(PaginationFilter filter, CancellationToken cancellationToken, int idUser = default)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            return await DbContext.Posts
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .Where(p => idUser == default || p.UserId == idUser)
               .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Return all posts by date
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="initial"></param>
        /// <param name="final"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetAllPostsByDateAsync(PaginationFilter filter, DateTime? initial, DateTime? final, CancellationToken cancellationToken)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var posts = await DbContext.Posts
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync(cancellationToken);

            return posts.Where(p => FilterDate(initial, final, p.DateInsert));
        }


        private static bool FilterDate(DateTime? initial, DateTime? final, DateTime current) => current switch
        {
            var d when initial.HasValue && !final.HasValue => d > initial.Value,
            var d when final.HasValue && !initial.HasValue => d < final.Value,
            var d when final.HasValue && initial.HasValue => d > initial.Value && d < final.Value,
            _ => false
        };

    }
}
