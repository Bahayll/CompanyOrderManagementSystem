using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Validators.Companies
{
    public class CreatCompanyRequestValidator : AbstractValidator<CreateCompanyCommandRequest>
    {
        public CreatCompanyRequestValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Name is required")
                .Length(1, 100)
                     .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(c => c.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .When(c => !string.IsNullOrEmpty(c.Email))
                   .WithMessage("Invalid email format.");




        }

    }
}
