namespace MyFace.Controllers
{
        [ApiController]
        [Route("/login")]
    public class LoginController : ControllerBase
    {
       
            private readonly IUsersRepo _users;

            public UsersController(IUsersRepo users)
            {
                _users = users;
            }

            [HttpGet("")]
            public ActionResult<UserListResponse> Search([FromQuery] UserSearchRequest searchRequest)
            {
                var users = _users.Search(searchRequest);
                var userCount = _users.Count(searchRequest);
                return UserListResponse.Create(searchRequest, users, userCount);
            }
        
        }
}