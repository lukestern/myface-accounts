using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyFace.Models.Request
{
    public static class PasswordRegEx
    {
        public const string Expression = 
           @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
    }
}
