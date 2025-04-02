using CompanyOrderManagement.Application.Features.Products.Commands.Update;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Validators.Products
{
    public class UpdateProductCommandRequestValidator : AbstractValidator<UpdateProductCommandRequest>
    {
        public UpdateProductCommandRequestValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                    .WithMessage("Company Id is required.");
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Name is required")
                .Length(1, 100)
                     .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(500)
                    .WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0)
                   .WithMessage("Stock must be a non-negative integer.");

            RuleFor(p => p.Price)
                .GreaterThan(0)
                   .WithMessage("Price must be greater than zero.");

            RuleFor(p => p.CompanyId)
                .NotNull()
                   .WithMessage("Company ID is required.");

            RuleFor(p => p.ProductCategoryId)
                .NotNull()
                   .WithMessage("Product Category ID is required.");
        }
    }
}
