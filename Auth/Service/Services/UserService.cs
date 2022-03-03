using Core.Interfaces;
using Core.Models.Dto;
using Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Dto;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<User>> CreateUserAsync(CreateUser createUser)
        {
            var user = new AppUser
            {
                Email = createUser.Email,
                UserName = createUser.UserName
            };

            var result = await _userManager.CreateAsync(user, createUser.Password);
            if (!result.Succeeded)
            {
                return Response<User>.Fail(new Error(result.Errors.Select(s => s.Description).ToList(), true), 404);
            }

            return Response<User>.Success(ObjectMapper.Mapper.Map<User>(user), 200);
        }

        public async Task<Response<User>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Response<User>.Fail("User not found.", 404, true);
            }

            return Response<User>.Success(ObjectMapper.Mapper.Map<User>(user), 200);
        }
    }
}