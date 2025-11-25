using FlowerBot.src.Data.Models.Database;
using FlowerBot.src.Data.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Data.Specifications
{
    public static class ProductQueries
    {
        public static IQueryable<Product> Active(this IQueryable<Product> query) => 
            query.Where(product => product.isActive && product.Quantity > 0);

        public static IEnumerable<ProductDto> ToDto(this IQueryable<Product> query, string baseUrl) =>
            query.Include(product => product.Image).AsEnumerable()
                .Select(product => new ProductDto()
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Category = product.Category,

                    Description = product.Description,
                    isActive = product.isActive,
                    Photo = product.Image != null && !string.IsNullOrEmpty(product.Image.Url)
                        ? $"{baseUrl}/{product.Image.Url.TrimStart('/')}"
                        : null,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                });
    }
}
