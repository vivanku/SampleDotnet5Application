using Sample.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Service.Interface
{
    public interface IUserService
    {
        Task<string> CreateUser(CreateUserModel model);
        Task<string> UpdateUser(UpdateUserModel model);
        Task<bool> DeleteUser(string id);
        Task<GetUserModel> GetUser(string id);
        Task<List<GetUserModel>> GetAllUsers();
        Task<AuthenticatedUserModel> AuthenticateUser(LoginModel model);
    }
}
