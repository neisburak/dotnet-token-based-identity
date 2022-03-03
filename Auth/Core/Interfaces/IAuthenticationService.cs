using Core.Models.Dto;
using Shared.Models.Dto;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<Token>> CreateTokenAsync(SignIn signIn);
        Task<Response<Token>> RefreshTokenAsync(string refreshToken);
        Task<Response<string>> RevokeRefreshTokenAsync(string refreshToken);

        Response<ClientToken> CreateToken(ClientSignIn clientSignIn);
    }
}