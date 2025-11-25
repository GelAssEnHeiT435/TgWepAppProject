using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Data.Models.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Data.Handlers
{
    public record class DeleteProductCommand(Guid Id): IRequest;
    
    public class DeleteProductHandler: IRequestHandler<DeleteProductCommand>
    {
        private readonly ApplicationContext _context;
        private readonly IFileManager _fileManager;

        public DeleteProductHandler(ApplicationContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _context.Products
                .Where(product => product.ProductId == request.Id)
                .Include(product => product.Image)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null) return;
            
            if(product.Image != null) await _fileManager.DeleteImageAsync(product.Image.Name!, cancellationToken);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
