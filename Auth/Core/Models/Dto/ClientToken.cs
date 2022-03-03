using System;

namespace Core.Models.Dto
{
    public class ClientToken
    {
        public string AccessToken { get; set; } = default!;
        public DateTime Expiration { get; set; }
    }
}