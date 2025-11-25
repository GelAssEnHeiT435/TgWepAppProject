using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Data.Handlers
{
    public record class UpdateProductCommand(Guid? Id, string Name, decimal Price,
                                             int Quantity, string Category, string Description, 
                                             bool IsActive, IFormFile? Image) : IRequest<ProductUpdateResult?>;

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

            if( !(string.IsNullOrWhiteSpace(request.Name) && product.Name.Equals(request.Name)) )
                product.Name = request.Name;

            if(product.Price != request.Price)
                product.Price = request.Price;

            if(product.Quantity != request.Quantity)
                product.Quantity = request.Quantity;

            if (!string.IsNullOrWhiteSpace(request.Category) && !request.Category.Equals(product.Category))
                product.Category = request.Category;

            if (!string.Equals(request.Description, product.Description, StringComparison.Ordinal))
                product.Description = request.Description;

            if(product.isActive != request.IsActive)
                product.isActive = request.IsActive;

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
