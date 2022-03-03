namespace Core.Models.Dto
{
    public class Product
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}