using System;

namespace Core.Models.Entities
{
    public class UserRefreshToken
    {
        public string UserId { get; set; } = default!;
        public string Token { get; set; } = default!;
        public DateTime Expiration { get; set; }
    }
}