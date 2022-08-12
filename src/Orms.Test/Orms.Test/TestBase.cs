using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orms.Api.Controllers;
using Orms.Domain.DTOs;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces;

namespace Orms.Tests
{
    public class TestBase
    {
        protected static void GetValidators(Mock<IUserRepository> userRepository, Mock<IPostRepository> postRepository, out InlineValidator<PostDto> validatorPost, out InlineValidator<PostDto> validatorRepost)
        {

            validatorPost = new InlineValidator<PostDto>();
            validatorPost.RuleFor(x => x).MustAsync(ValidatePostByDayAsync(userRepository.Object)).WithMessage("It's possible insert only 5 post by day")
                            .MustAsync(ValidateUserDbAsync(userRepository.Object)).WithMessage("The user must have exits in database");

            validatorRepost = new InlineValidator<PostDto>();
            validatorRepost.RuleFor(x => x)
            .MustAsync(ValidatePostByDayAsync(userRepository.Object)).WithMessage("It's possible insert only 5 post by day")
            .MustAsync(async (repost, cancellation) =>
            {
                var post = await postRepository.Object.GetPostByIdAsync(1, cancellation);

                return post != default(Post);
            }).WithMessage("The post must have exits in database")
            .MustAsync(ValidateUserDbAsync(userRepository.Object)).WithMessage("The user must have exits in database");
        }

        protected static Func<PostDto, CancellationToken, Task<bool>> ValidatePostByDayAsync(IUserRepository userRepo)
        {
            return async (repost, cancellation) =>
            {
                //A user is not allowed to post more than 5 posts in one day
                var countPosts = await userRepo.CountPostsByUserAsync(repost.UserId, cancellation);


                return (countPosts < 5);

            };
        }

        protected static  Func<PostDto, CancellationToken, Task<bool>> ValidateUserDbAsync(IUserRepository userRepo)
        {
            return async (post, cancellation) =>
            {
                var user = await userRepo.GetUserAsync(post.UserId, cancellation);

                return user != default(User);
            };
        }


        protected (Mock<IPostRepository>, Mock<IUserRepository>, PostController, UserController, DefaultHttpContext) GetHockRepository()
        {

            var httpContext = new DefaultHttpContext();
            var mockPost = new Mock<IPostRepository>();
            var mockUser = new Mock<IUserRepository>();


            GetValidators(mockUser, mockPost, out InlineValidator<PostDto> validatorPost, out InlineValidator<PostDto> validatorRepost);

            var postController = new PostController(mockPost.Object, validatorPost, validatorRepost)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };


            var userController = new UserController(mockUser.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            return (mockPost, mockUser, postController, userController, httpContext);

        }
    }
}