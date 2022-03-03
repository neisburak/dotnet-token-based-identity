using Core.Models.Business;
using Core.Models.Dto;
using Core.Models.Entities;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Token CreateToken(AppUser user);
        ClientToken CreateToken(Client client);
    }
}