namespace FlowerBot.src.Data.Models.Dto
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
