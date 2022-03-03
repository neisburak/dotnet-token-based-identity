using Core.Models.Dto;
using Shared.Models.Dto;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<Response<User>> CreateUserAsync(CreateUser createUser);
        Task<Response<User>> GetUserByNameAsync(string userName);
    }
}