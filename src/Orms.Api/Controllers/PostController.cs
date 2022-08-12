using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Orms.Domain.DTOs;
using Orms.Domain.Entities;
using Orms.Domain.Entities.Pagination;
using Orms.Domain.Enuns;
using Orms.Domain.Interfaces;
using System.Net;

namespace Orms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        /// <summary>
        /// Rules PostDto
        /// </summary>
        private readonly IValidator<PostDto> _validatorPost;

        /// <summary>
        /// Rules PostDtoRepost
        /// </summary>
        private readonly IValidator<PostDtoRepost> _validatorRepost;

        /// <summary>
        /// Repository of posts
        /// </summary>
        private readonly IPostRepository _postRepo;


        public PostController(IPostRepository postRepo,
                                IValidator<PostDto> validatorPost,
                                IValidator<PostDtoRepost> validatorRepost
                                )
        {
            _postRepo = postRepo;
            _validatorPost = validatorPost;
            _validatorRepost = validatorRepost;
        }
        /// <summary>
        ///  Posts are loaded on-demand on chunks of 10 posts, the frontend will pass values.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [Route("getAllPosts")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter)
        {
            var posts = await _postRepo.GetAllPostAsync(filter, HttpContext.RequestAborted);
            return Ok(new Response<IEnumerable<Post>>(posts));
        }

        /// <summary>
        ///  Posts are loaded on-demand on chunks of 5 posts per user, the frontend will pass values.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [Route("getAllPostsOnlyMe")]
        public async Task<IActionResult> GetOnlyMe([FromQuery] PaginationFilter filter, int idUser)
        {
            var posts = await _postRepo.GetAllPostAsync(filter, HttpContext.RequestAborted, idUser);
            return Ok(new Response<IEnumerable<Post>>(posts));
        }

        /// <summary>
        ///  Posts are loaded on-demand on chunks of 10 posts,the frontend will pass values.
        ///  Date range filter option (start date and end date) that allows results filtering based on the posted date, both values are optional the frontend will pass values.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [Route("getAllPostsByDate")]
        public async Task<IActionResult> GetAllPostsByDate([FromQuery] PaginationFilter filter, DateTime? initial, DateTime? final)
        {
            var posts = await _postRepo.GetAllPostsByDateAsync(filter, initial, final, HttpContext.RequestAborted);
            return Ok(new Response<IEnumerable<Post>>(posts));
        }

        /// <summary>
        /// Create post, a user is not allowed to post more than 5 posts in one day (including reposts and quote posts).
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [Route("createPost")]
        public async Task<IActionResult> CreatePost(PostDto post)
        {
            var validation = await _validatorPost.ValidateAsync(post);

            if (!validation.IsValid)
                return BadRequest(validation.Errors?.Select(e => new ValidationResultDto(e.ErrorCode, e.PropertyName, e.ErrorMessage)));

            var idPost = await _postRepo.CreatePostUsersAsync(post, HttpContext.RequestAborted);

            return Ok(idPost);
        }


        /// <summary>
        /// User can create reposting and quote using this method, when it's quote, frontend will pass the parameter 'isQuote' with value true
        /// </summary>
        /// <param name="postDto"></param>
        /// <param name="isQuote"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [Route("repost")]
        public async Task<IActionResult> Reposting(PostDtoRepost postDto, bool isQuote = false)
        {
            if (isQuote)
            {
                if (postDto.Type != PostType.Original && postDto.Type != PostType.Repost)
                    return BadRequest(new ValidationResultDto("Invalid type", "Type", "Quote is limited to original and reposts posts"));
            }
            else
            {
                if (postDto.Type != PostType.Original && postDto.Type != PostType.Quote)
                    return BadRequest(new ValidationResultDto("Invalid type", "Type", "Reposting is limited to original and quote posts"));
            }

            var validation = await _validatorRepost.ValidateAsync(postDto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors?.Select(e => new ValidationResultDto(e.ErrorCode, e.PropertyName, e.ErrorMessage)));

            var idPost = await _postRepo.CreateRepostAsync(postDto, HttpContext.RequestAborted);

            return Ok(idPost);
        }
    }
}
