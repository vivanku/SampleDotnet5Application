using Sample.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Model
{

    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
    }
    public class CreateUserModel : UserModel
    {
        public string Password { get; set; }
    }
    public class UpdateUserModel : UserModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }

    public class GetUserModel : UserModel
    {
        public string Id { get; set; }
    }
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class AuthenticatedUserModel : GetUserModel
    {
        public string Token { get; set; }
    }
}
