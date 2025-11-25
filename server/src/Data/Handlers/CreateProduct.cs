using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Database;
using MediatR;

namespace FlowerBot.src.Data.Handlers
{
    public record class CreateProductCommand(string Name, decimal Price, int Quantity, 
                                             string Category, string Description, bool isActive, 
                                             IFormFile? Image) : IRequest<ProductCreateResult>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductCreateResult>
    {
        private readonly ApplicationContext _context;
        private readonly IFileManager _fileManager;

        public CreateProductCommandHandler(ApplicationContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<ProductCreateResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            ImageUploadResult? ImageData = await _fileManager.SaveImageAsync(request.Image, cancellationToken);

            Product product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
                Category = request.Category,
                Description = request.Description,
                isActive = request.isActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Image = ImageData?.RelativePath != null
                    ? new Image
                    {
                        Name = ImageData?.FileName!,
                        Url = ImageData?.RelativePath!
                    } : null
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return new ProductCreateResult(product.ProductId, ImageData?.RelativePath);
        }
    }
}
