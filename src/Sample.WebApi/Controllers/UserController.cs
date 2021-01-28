using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Model;
using Sample.Application.Service.Interface;
using Sample.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.WebApi.Controllers
{
    /// <summary>
    /// Manage Users
    /// </summary>
    public class UserController : ApiControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CreateUser(CreateUserModel model) => await _userService.CreateUser(model);

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<List<GetUserModel>> GetAllUsers() => await _userService.GetAllUsers();

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<GetUserModel> GetUser(string id) => await _userService.GetUser(id);

        /// <summary>
        /// Authenticate User Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<AuthenticatedUserModel> AuthenticateUser(LoginModel model) => await _userService.AuthenticateUser(model);


    }
}
