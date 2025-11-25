using Microsoft.AspNetCore.Mvc;

namespace FlowerBot.src.Data.Models.Dto
{
    public class InnerProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
