using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sample.Application.Model;
using Sample.Application.Repository;
using Sample.Application.Service.Interface;
using Sample.Domain.Entity;
using Sample.Domain.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;


namespace Sample.Application.Service.Implementation
{
    public class UserService : IUserService
    {
        private IGenericRepository<User> _userRepository;
        private AppConfigOptions _appSettingsOption;

        public UserService(IGenericRepository<User> userRepository, IOptions<AppConfigOptions> appSettingsOption)
        {
            _userRepository = userRepository;
            _appSettingsOption = appSettingsOption.Value;
        }
        public async Task<string> CreateUser(CreateUserModel model)
        {
            IEnumerable<User> users = await _userRepository.GetAll();
            User existingUser = users.Where(x => x.Email == model.Email).SingleOrDefault();
            if (existingUser != null)
                throw new ValidationException("Email already exist");
            User user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PasswordSalt = BC.GenerateSalt(),
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow

            };
            user.Password = BC.HashPassword(model.Password, user.PasswordSalt);
            await _userRepository.Add(user);
            return user.Id;
        }
        public async Task<bool> DeleteUser(string id)
        {

            User user = await _userRepository.Get(id);
            await _userRepository.Delete(user);
            return true;
        }

        public async Task<List<GetUserModel>> GetAllUsers()
        {
            List<GetUserModel> userModelList = new List<GetUserModel>();
            IEnumerable<User> users = await _userRepository.GetAll();
            foreach (User user in users)
            {
                userModelList.Add(new GetUserModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }
            return userModelList;
        }

        public async Task<GetUserModel> GetUser(string id)
        {
            User user = await _userRepository.Get(id);
            return new GetUserModel() 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<string> UpdateUser(UpdateUserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticatedUserModel> AuthenticateUser(LoginModel model)
        {
            IEnumerable<User> users = await _userRepository.GetAll();
            User user = users.Where(x => x.Email == model.Email).SingleOrDefault();
            if (user == null)
                throw new ValidationException("User Doesnot Exist");

            string passwordHash = BC.HashPassword(model.Password, user.PasswordSalt);
            if(!passwordHash.Equals(user.Password))
                throw new ValidationException("Incorrect password");

            var token = GenerateJwtToken(user);
            return new AuthenticatedUserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Token = GenerateJwtToken(user)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettingsOption.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
