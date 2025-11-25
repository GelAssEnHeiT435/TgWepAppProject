using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Dto;
using FlowerBot.src.Data.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Data.Handlers
{
    public record class ReadProductsCommand(string url): IRequest<IReadOnlyCollection<ProductDto>>;

    public class ReadProductsHandler : IRequestHandler<ReadProductsCommand, IReadOnlyCollection<ProductDto>>
    {
        private readonly ApplicationContext _context;

        public ReadProductsHandler(ApplicationContext context) => _context = context;

        public async Task<IReadOnlyCollection<ProductDto>> Handle(ReadProductsCommand request, CancellationToken cancellationToken) =>
             _context.Products
                .AsNoTracking()
                .ToDto(request.url)
                .ToList();
    }
}
