using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetAll;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyOrderManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
       [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllProductCategoryQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _mediator.Send(new GetByIdProductCategoryQueryRequest(Guid.Parse(id)));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCategoryCommandRequest createProductCategoryCommandRequest)
        {
            var response = await _mediator.Send(createProductCategoryCommandRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductCategoryCommandRequest updateProductCategoryCommandRequest)
        {
            var response = await _mediator.Send(updateProductCategoryCommandRequest);
            return Ok(response);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteProductCategoryCommandRequest(Guid.Parse(id)));
            return Ok();
        }

    }
}
