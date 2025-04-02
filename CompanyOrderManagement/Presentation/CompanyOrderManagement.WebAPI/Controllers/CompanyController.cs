using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.Commands.Delete;
using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CompanyOrderManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllCompanyQueryRequest());
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _mediator.Send(new GetByIdCompanyQueryRequest(Guid.Parse(id)));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommandRequest createCompanyCommandRequest)
        {
            var response = await _mediator.Send(createCompanyCommandRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCompanyCommandRequest updateCompanyCommandRequest)
        {
            var response= await _mediator.Send(updateCompanyCommandRequest);
            return Ok(response);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteCompanyCommandRequest(Guid.Parse(id)));
            return Ok();
        }
    }
}
