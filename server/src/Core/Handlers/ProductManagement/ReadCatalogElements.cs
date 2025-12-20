using FlowerBot.src.Data;
using FlowerBot.src.Data.Models.Dto;
using FlowerBot.src.Data.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowerBot.src.Core.Handlers.ProductManagement
{
    public record class ReadCatalogElementsCommand(string baseUrl): IRequest<IReadOnlyCollection<ProductDto>>;

    public class ReadCatalogElementsHandler : IRequestHandler<ReadCatalogElementsCommand, IReadOnlyCollection<ProductDto>>
    {
        private readonly ApplicationContext _context;

        public ReadCatalogElementsHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<ProductDto>> Handle(ReadCatalogElementsCommand request, CancellationToken cancellationToken) =>
            _context.Products
                .AsNoTrackingWithIdentityResolution()
                .Active()
                .ToDto(request.baseUrl)
                .ToList();
    }
}
