using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.Commands.Create;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Validators.Orders
{
    public class CreateOrderCommandRequestValidator : AbstractValidator<CreateOrderCommandRequest>
    {
        public CreateOrderCommandRequestValidator() 
        {
            RuleFor(o => o.Name)
                .NotEmpty()
                .NotNull()
                   .WithMessage("Name is required")
                .Length(1, 100)
                   .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(o => o.Description)
               .MaximumLength(500)
                   .WithMessage("Description must not exceed 500 characters.");

            RuleFor(o => o.Address)
                .MaximumLength(250)
                   .WithMessage("Address must not exceed 250 characters.");

            RuleFor(o => o.ProductCount)
                .GreaterThanOrEqualTo(0)
                   .WithMessage("Product count must be a non-negative integer.");

            RuleFor(o => o.UnitPrice)
                .GreaterThan(0)
                   .WithMessage("Unit price must be greater than zero.");
           

        }
    }
}
