using FluentValidation;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces;

namespace Orms.Domain.DTOs
{
    public class PostDtolValidatorBase<T> : AbstractValidator<T> where T: PostDto
    {
        protected static Func<T, CancellationToken, Task<bool>> ValidatePostByDayAsync(IUserRepository userRepo)
        {
            return async (repost, cancellation) =>
            {
                //A user is not allowed to post more than 5 posts in one day
                var countPosts = await userRepo.CountPostsByUserAsync(repost.UserId, cancellation);

                return (countPosts < 5);

            };
        }

        protected Func<PostDto, CancellationToken, Task<bool>> ValidateUserDbAsync(IUserRepository userRepo)
        {
            return async (post, cancellation) =>
            {
                var user = await userRepo.GetUserAsync(post.UserId, cancellation);

                return user != default(User);
            };
        }
    }
}
