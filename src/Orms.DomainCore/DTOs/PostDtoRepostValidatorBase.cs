using FluentValidation;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces;

namespace Orms.Domain.DTOs
{
    public class PostDtoRepostValidator : PostDtolValidatorBase<PostDtoRepost>
    {
        public PostDtoRepostValidator(IUserRepository userRepo, IPostRepository postRepo)
        {
            RuleFor(x => x)
            .MustAsync(ValidatePostByDayAsync(userRepo)).WithMessage("It's possible insert only 5 post by day")
            .MustAsync(async (repost, cancellation) =>
            {
                var post = await postRepo.GetPostByIdAsync(repost.PostId, cancellation);

                return post != default(Post);
            }).WithMessage("The post must have exits in database")
            .MustAsync(ValidateUserDbAsync(userRepo)).WithMessage("The user must have exits in database");
        }
    }
}
