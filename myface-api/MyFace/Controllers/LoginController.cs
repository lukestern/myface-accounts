using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using System;


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
        public ActionResult<LoginResponse> Auth()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) && authHeader.ToString().StartsWith("Basic"))
            {
                var usernamePassword = AuthHelper.GetUsernamePasswordFromAuthHeader(authHeader);
                var user = _users.GetByUsername(usernamePassword[0]);
                var isLoggedIn = _users.HasAccess(user, usernamePassword[1]);
                return new LoginResponse(user, isLoggedIn);
            }
            return new NotFoundResult();
        }

        private object GetByUsername(string v) => throw new System.NotImplementedException();
    }
}