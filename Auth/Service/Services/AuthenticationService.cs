using Core.Interfaces;
using Core.Models.Business;
using Core.Models.Dto;
using Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Models.Dto;

namespace Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly List<Client> _clients;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

        public AuthenticationService(UserManager<AppUser> userManager, ITokenService tokenService, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenRepository, IOptions<List<Client>> clients)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _clients = clients.Value;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        public async Task<Response<Token>> CreateTokenAsync(SignIn signIn)
        {
            if (signIn == null)
            {
                throw new ArgumentNullException(nameof(signIn));
            }

            var user = await _userManager.FindByEmailAsync(signIn.Email);
            if (user == null)
            {
                return Response<Token>.Fail("Email or password is invalid.", 404, true);
            }

            if (!await _userManager.CheckPasswordAsync(user, signIn.Password))
            {
                return Response<Token>.Fail("Email or password is invalid.", 404, true);
            }

            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenRepository.Where(w => w.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    Token = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.Token = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<Token>.Success(token, 200);
        }

        public Response<ClientToken> CreateToken(ClientSignIn clientSignIn)
        {
            var client = _clients.SingleOrDefault(s => s.Id == clientSignIn.ClientId && s.Secret == clientSignIn.ClientSecret);

            if (client == null)
            {
                return Response<ClientToken>.Fail("Client credentials are invalid.", 404, true);
            }

            var token = _tokenService.CreateToken(client);
            return Response<ClientToken>.Success(token, 200);
        }

        public async Task<Response<Token>> RefreshTokenAsync(string refreshToken)
        {
            var entity = await _userRefreshTokenRepository.Where(w => w.Token == refreshToken).SingleOrDefaultAsync();
            if (entity == null)
            {
                return Response<Token>.Fail("Refresh token not found.", 404, true);
            }

            var user = await _userManager.FindByIdAsync(entity.UserId);
            if (user == null)
            {
                return Response<Token>.Fail("User not found.", 404, true);
            }

            var token = _tokenService.CreateToken(user);

            entity.Token = token.RefreshToken;
            entity.Expiration = token.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<Token>.Success(token, 200);
        }

        public async Task<Response<string>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var entity = await _userRefreshTokenRepository.Where(w => w.Token == refreshToken).SingleOrDefaultAsync();
            if (entity == null)
            {
                return Response<string>.Fail("Refresh token not found.", 404, true);
            }

            _userRefreshTokenRepository.Remove(entity);

            await _unitOfWork.CommitAsync();

            return Response<string>.Success(200);
        }
    }
}