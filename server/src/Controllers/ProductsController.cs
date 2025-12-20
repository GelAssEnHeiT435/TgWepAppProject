using FlowerBot.src.Core.Handlers.ProductManagement;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlowerBot.src.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> CreateProduct(
            [FromForm] CreateProductDto product,
            IFormFile? Image, CancellationToken ct = default)
        {
            CreateProductCommand query = new CreateProductCommand(
                product.Name, product.Price, product.Quantity, product.Category, 
                product.Description, product.IsActive, Image);
            ProductCreateResult result = await _mediator.Send(query, ct);

            string? absoluteImageUrl = result.relativeUrl != null
                ? $"{Request.Scheme}://{Request.Host}{result.relativeUrl}"
                : null;

            return Ok(new {
                id = result.Id, 
                image = absoluteImageUrl
            });
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateProduct(
            [FromQuery] Guid Id,
            [FromForm] UpdateProductDto product,
            IFormFile? Image, CancellationToken ct = default)
        {
            UpdateProductCommand query = new UpdateProductCommand(
                Id, product.Name, product.Price, product.Quantity, product.Category,
                product.Description, product.IsActive, Image);
            ProductUpdateResult? result = await _mediator.Send(query, ct);

            string? AbsoluteUrl = !string.IsNullOrWhiteSpace(result?.relativePath) ? $"{Request.Scheme}://{Request.Host}{result.relativePath}" : null;
            return Ok( new {
                image = AbsoluteUrl,
                update = result?.updatedTime
            });
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetAllProducts(
            CancellationToken ct = default)
        {
            string url = $"{Request.Scheme}://{Request.Host}";

            ReadProductsCommand query = new ReadProductsCommand(url);
            IReadOnlyCollection<ProductDto> products = await _mediator.Send(query, ct);
            return Ok(products);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(
            [FromQuery] Guid Id,
            CancellationToken ct = default)
        {
            DeleteProductCommand query = new DeleteProductCommand(Id);
            await _mediator.Send(query, ct);
            return Ok();
        }
    }
}
