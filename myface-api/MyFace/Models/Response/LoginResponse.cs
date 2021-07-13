using MyFace.Models.Database;

namespace MyFace.Models.Response
{
    public class LoginResponse
    {
        private readonly User _user;
        public bool IsLoggedIn;

        public LoginResponse(User user, bool isLoggedIn)
        {
            _user = user;
            IsLoggedIn = isLoggedIn;
        }

        public int Id => _user.Id;
        public string Username => _user.Username;
    }
}