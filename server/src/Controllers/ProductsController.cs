using FlowerBot.src.Core.Interfaces;
using FlowerBot.src.Core.Services;
using FlowerBot.src.Data.Handlers;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace FlowerBot.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct(
            [FromForm] InnerProductDto product,
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

        [HttpPut("update")]
        public async Task<ActionResult> UpdateProduct(
            [FromForm] InnerProductDto product,
            IFormFile? Image, CancellationToken ct = default)
        {
            UpdateProductCommand query = new UpdateProductCommand(
                product.Id, product.Name, product.Price, product.Quantity, product.Category,
                product.Description, product.IsActive, Image);
            ProductUpdateResult? result = await _mediator.Send(query, ct);

            string? AbsoluteUrl = !string.IsNullOrWhiteSpace(result?.relativePath) ? $"{Request.Scheme}://{Request.Host}{result.relativePath}" : null;
            return Ok( new {
                image = AbsoluteUrl,
                update = result?.updatedTime
            });
        }

        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetAllProducts(
            CancellationToken ct = default)
        {
            string url = $"{Request.Scheme}://{Request.Host}";

            ReadProductsCommand query = new ReadProductsCommand(url);
            IReadOnlyCollection<ProductDto> products = await _mediator.Send(query, ct);
            return Ok(products);
        }

        [HttpDelete("delete")]
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
