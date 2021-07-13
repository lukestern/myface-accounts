using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo _users;

        public UsersController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<UserListResponse> Search([FromQuery] UserSearchRequest searchRequest)
        {
            System.Collections.Generic.IEnumerable<Models.Database.User> users = _users.Search(searchRequest);
            int userCount = _users.Count(searchRequest);
            return UserListResponse.Create(searchRequest, users, userCount);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetById([FromRoute] int id)
        {
            Microsoft.Extensions.Primitives.StringValues authHeader = HttpContext.Request.Headers["Authorization"];
            //_users.UserHasAccess(authHeader)
            Models.Database.User user = _users.GetById(id);
            return new UserResponse(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateUserRequest newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.Database.User user = _users.Create(newUser);

            string url = Url.Action("GetById", new { id = user.Id });
            UserResponse responseViewModel = new UserResponse(user);
            return Created(url, responseViewModel);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<UserResponse> Update([FromRoute] int id, [FromBody] UpdateUserRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.Database.User user = _users.Update(id, update);
            return new UserResponse(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _users.Delete(id);
            return Ok();
        }
    }
}