using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Validators.ProductCategories
{
    public class UpdateProductCategoryCommandRequestValidator : AbstractValidator<UpdateCompanyCommandRequest>
    {
        public UpdateProductCategoryCommandRequestValidator()
        {

            RuleFor(p => p.Id)
                .NotEmpty()
                   .WithMessage("Id is required.");

            RuleFor(p => p.Name)
                .NotEmpty()
                   .WithMessage("Name is required.")
            .Length(1, 100)
                   .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(500)
                   .WithMessage("Description must not exceed 500 characters.");
        }
    }
}
