namespace Core.Models.Business
{
    public class Client
    {
        public string Id { get; set; } = default!;
        public string Secret { get; set; } = default!;
        public List<string> Audiences { get; set; } = new();
    }
}