using Microsoft.AspNetCore.Mvc;

namespace FlowerBot.src.Data.Models.Dto
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
