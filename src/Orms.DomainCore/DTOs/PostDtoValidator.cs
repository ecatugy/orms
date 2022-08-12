using FluentValidation;
using Orms.Domain.Interfaces;

namespace Orms.Domain.DTOs
{
    public class PostDtolValidator : PostDtolValidatorBase<PostDto> 
    {
        public PostDtolValidator(IUserRepository userRepo)
        {
            RuleFor(x => x).MustAsync(ValidatePostByDayAsync(userRepo)).WithMessage("It's possible insert only 5 post by day")
                            .MustAsync(ValidateUserDbAsync(userRepo)).WithMessage("The user must have exits in database");
            RuleFor(model => model.Content).NotNull().NotEmpty().Length(0, 777);

        }
    }
}
