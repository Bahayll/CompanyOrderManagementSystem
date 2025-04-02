using CompanyOrderManagement.Application.Features.Orders.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.Commands.Delete;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyOrderManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllOrderQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _mediator.Send(new GetByIdOrderQueryRequest(Guid.Parse(id)));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommandRequest createOrderCommandRequest)
        {
            var response = await _mediator.Send(createOrderCommandRequest);
            return Ok(response);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteOrderCommandRequest(Guid.Parse(id)));
            return Ok();
        }


    }
}
