using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {

        private readonly IUsersRepo _users;

        public LoginController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<LoginResponse> Auth([FromQuery] UserSearchRequest searchRequest)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            var isLoggedIn = _users.HasAccess(authHeader);
            var user = _users.GetByUsername(AuthHelper.GetUsernamePasswordFromAuthHeader(authHeader)[0]);
            return new LoginResponse(user, isLoggedIn);
        }

    }
}