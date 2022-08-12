using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orms.Api.Controllers;
using Orms.Domain.DTOs;
using Orms.Domain.Entities;
using Orms.Domain.Entities.Pagination;
using Orms.Domain.Interfaces;
using Orms.Tests.Mocks;

namespace Orms.Tests
{
    public class PostTest : TestBase
    {
        [Fact]
        public async Task Get_GetAllPosts()
        {
            var filter = new PaginationFilter(1, 10);
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var (mock, _, postController,_, context) = GetHockRepository();

            mock.Setup(m => m.GetAllPostAsync(filter, It.IsAny<CancellationToken>(), default)).ReturnsAsync(() =>
                Helpers.GetListPost().Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToList())
                .Verifiable();


            var result = await postController.Get(filter) as ObjectResult;
            var value = result?.Value as Response<IEnumerable<Post>>;


            Assert.IsType<OkObjectResult>(result);
            //latest 10 posts
            Assert.Equal(10, value?.Data?.Count());

        }

        [Fact]
        public async Task Get_GetOnlyMe()
        {

            var filter = new PaginationFilter(1, 10);
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var isMe = 1;

            var (mock, _, postController,_, context) = GetHockRepository();

            mock.Setup(m => m.GetAllPostAsync(filter, It.IsAny<CancellationToken>(), isMe)).ReturnsAsync(() =>
                Helpers.GetListPost().Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .Where(p => isMe == default || p.UserId == isMe)
               .ToList())
               .Verifiable();


            var result = await postController.GetOnlyMe(filter, isMe) as ObjectResult;
            var value = result?.Value as Response<IEnumerable<Post>>;


            Assert.IsType<OkObjectResult>(result);
            //All post from me
            Assert.True(value?.Data?.All(p => p.UserId == isMe));
        }


        [Fact]
        public async Task Get_GetAllPostsByDate()
        {
            var filter = new PaginationFilter(1, 10);
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            DateTime? initialDate = DateTime.Now.AddDays(-10);
            DateTime? finalDate = null;

            var (mock, _, postController,_, context) = GetHockRepository();

            mock.Setup(m => m.GetAllPostsByDateAsync(filter, initialDate, finalDate, It.IsAny<CancellationToken>())).ReturnsAsync(
                        Helpers.GetListPost().Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .Where(p => FilterDate(initialDate, finalDate, p.DateInsert)).ToList())
                       .Verifiable();


            var result = await postController.GetAllPostsByDate(filter, initialDate, finalDate) as ObjectResult;
            var value = result?.Value as Response<IEnumerable<Post>>;


            Assert.IsType<OkObjectResult>(result);
            //latest 10 posts, the begin date was created 10 days after date the posts
            Assert.Equal(10, value?.Data?.Count());
        }


        [Fact]
        public async Task Post_CreatePost()
        {
            var (mock, mockUser, postController,_, _) = GetHockRepository();
            var content = @"content-teste";
            var idUser = 1;

            var itens = Helpers.GetListPost(10);

            SetMockInsert(mock, mockUser, idUser, itens);

            var dto = new PostDto(Domain.Enuns.PostType.Original, content, idUser);

            var result = await postController.CreatePost(dto) as ObjectResult;

            //Ensure 200 response
            Assert.IsType<OkObjectResult>(result);
            //Check if item was inserted
            Assert.Equal(11, itens?.Count);
        }


        [Fact]
        public async Task Post_Reposting()
        {
            var (mock, mockUser, postController,_, _) = GetHockRepository();
            var idUser = 1;
            var itens = Helpers.GetListPost(40);

            var repost = itens.First(p => p.Type == Domain.Enuns.PostType.Repost);

            SetMockInsert(mock, mockUser, idUser, itens);

            var dto = new PostDtoRepost(repost.Type, repost.Content, repost.Comment, repost.UserId, repost.PostId);

            var result = await postController.Reposting(dto) as ObjectResult;
            var value = result?.Value as ValidationResultDto;

            Assert.IsType<BadRequestObjectResult>(result);
            //Check if item was inserted
            Assert.Equal("Reposting is limited to original and quote posts", value?.Message);
        }

        private static void SetMockInsert(Mock<IPostRepository>? mock, Mock<IUserRepository>? mockUser, int idUser, List<Post> itens)
        {
            mockUser?.Setup(m => m.GetUserAsync(idUser, It.IsAny<CancellationToken>())).ReturnsAsync(
                      Helpers.GetUsers()
                     .Find(p => p.UserID == idUser))
                     .Verifiable();

            //Set list post with 10 itens
            mockUser?.Setup(m => m.CountPostsByUserAsync(idUser, It.IsAny<CancellationToken>())).ReturnsAsync(
                  itens
                 .Count(p => p.UserId == idUser && p.DateInsert < DateTime.Now.AddHours(-24)))
                 .Verifiable();

            mock?.Setup(m => m.CreatePostUsersAsync(It.IsAny<PostDto>(), It.IsAny<CancellationToken>()))
                  .Callback((PostDto postDto, CancellationToken cancellationToken) => { itens.Add(new Post { Type = postDto.Type, Comment = postDto.Content, UserId = postDto.UserId, PostId = Helpers.GetListPost(10).Max(p => p.PostId) + 1 }); });
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