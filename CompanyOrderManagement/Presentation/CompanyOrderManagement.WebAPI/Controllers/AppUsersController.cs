using CompanyOrderManagement.Application.Features.AppUsers.Commands.Create;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Delete;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Login;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Update;
using CompanyOrderManagement.Application.Features.AppUsers.Queries.GetAll;
using CompanyOrderManagement.Application.Features.AppUsers.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompanyOrderManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public AppUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllAppUserQueryRequest());
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginAppUserCommandRequest loginAppUserCommandRequest)
        {
            var response = await _mediator.Send(loginAppUserCommandRequest);
            return Ok(response);
        }
        [HttpPost]
        public async Task <IActionResult> Create([FromBody] CreateAppUserCommandRequest createAppUserCommandRequest)
        {
            var apiResponse = await _mediator.Send(createAppUserCommandRequest);

               return Ok(apiResponse);
          
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteAppUserCommandRequest(Guid.Parse(id)));
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAppUserCommandRequest updateAppUserCommandRequest)
        {
            var response = await _mediator.Send(updateAppUserCommandRequest);
            return Ok(response);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _mediator.Send(new GetByIdAppUserQueryRequest(Guid.Parse(id)));
            return Ok(response);
        }
    }
}
