using Microsoft.AspNetCore.Mvc;
using Orms.Domain.Entities;
using Orms.Domain.Interfaces;
using System.Net;

namespace Orms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Repository of users
        /// </summary>
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        /// <summary>
        /// Return user by id with all posts that user has made (including reposts and quote posts) 
        /// This method return a list for post, the frontend will count for show in page. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [Route("getUser")]
        public async Task<IActionResult> Get(int id)
        {

            var users = await _userRepo.GetUserAsync(id, HttpContext.RequestAborted);

            return Ok(users);
        }
    }
}

