using FlowerBot.src.Core.Handlers.ProductManagement;
using FlowerBot.src.Data.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerBot.src.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetCatalogList(
            CancellationToken cancellationToken)
        {
            string url = $"{Request.Scheme}://{Request.Host}";

            ReadCatalogElementsCommand query = new ReadCatalogElementsCommand(url);
            IReadOnlyCollection<ProductDto> products = await _mediator.Send(query, cancellationToken);

            return Ok(products);
        }
    }
}
