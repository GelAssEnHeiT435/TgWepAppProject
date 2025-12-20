using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Core.Handlers.ProductManagement
{
    public record class UpdateProductCommand(Guid? Id, string? Name, decimal? Price,
                                             int? Quantity, string? Category, string? Description, 
                                             bool? IsActive, IFormFile? Image) : IRequest<ProductUpdateResult?>;

    public class UpdatePriductHandler : IRequestHandler<UpdateProductCommand, ProductUpdateResult?>
    {
        private readonly ApplicationContext _context;
        private readonly IFileManager _fileManager;

        public UpdatePriductHandler(ApplicationContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<ProductUpdateResult?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _context.Products
                .Include(product => product.Image)
                .Where(product => product.ProductId == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null) return null;

            string? newRelativePath = null;
            if (request.Image != null && request.Image.Length > 0)
            {
                if (product.Image != null)
                    await _fileManager.DeleteImageAsync(product.Image.Name!, cancellationToken);

                ImageUploadResult? imageResult = await _fileManager.SaveImageAsync(request.Image, cancellationToken);
                product.Image.Name = imageResult.FileName;
                product.Image.Url = imageResult.RelativePath;

                newRelativePath = imageResult.RelativePath;
            }

            if (!string.IsNullOrWhiteSpace(request.Name) && !string.Equals(product.Name, request.Name, StringComparison.Ordinal))
                product.Name = request.Name;

            if (request.Price.HasValue && product.Price != request.Price.Value)
                product.Price = request.Price.Value;

            if (request.Quantity.HasValue && product.Quantity != request.Quantity.Value)
                product.Quantity = request.Quantity.Value;

            if (!string.IsNullOrWhiteSpace(request.Category) && !string.Equals(product.Category, request.Category, StringComparison.Ordinal))
                product.Category = request.Category;

            if (request.Description != null && !string.Equals(product.Description, request.Description, StringComparison.Ordinal))
                product.Description = request.Description;

            if (request.IsActive.HasValue && product.isActive != request.IsActive.Value)
                product.isActive = request.IsActive.Value;

            bool hasChanges = _context.Entry(product).Properties
                .Any(p => p.IsModified && p.OriginalValue != null && !Equals(p.OriginalValue, p.CurrentValue));
            bool hasChangesImage = _context.Entry(product.Image).Properties
                .Any(p => p.IsModified && p.OriginalValue != null && !Equals(p.OriginalValue, p.CurrentValue));

            if (hasChanges || hasChangesImage)
                product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return new ProductUpdateResult(newRelativePath, product.UpdatedAt);
        }
    }
}
