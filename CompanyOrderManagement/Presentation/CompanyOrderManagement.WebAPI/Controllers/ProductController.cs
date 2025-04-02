using CompanyOrderManagement.Application.Features.Products.Commands.Create;
using CompanyOrderManagement.Application.Features.Products.Commands.Delete;
using CompanyOrderManagement.Application.Features.Products.Commands.Update;
using CompanyOrderManagement.Application.Features.Products.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Products.Queries.GetById;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyOrderManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllProductQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _mediator.Send(new GetByIdProductQueryRequest(Guid.Parse(id)));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommandRequest createProductCommandRequest)
        {
            var response = await _mediator.Send(createProductCommandRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductCommandRequest updateProductCommandRequest)
        {
            var response = await _mediator.Send(updateProductCommandRequest);
            return Ok(response);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteProductCommandRequest(Guid.Parse(id)));
            return Ok();
        }

    }

}

