using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompanyOrderManagement.Application.Validators.Companies
{
    public class UpdateCompanyCommandRequestValidator : AbstractValidator<UpdateCompanyCommandRequest>
    {
        public UpdateCompanyCommandRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                   .WithMessage("Id is required.");

            RuleFor(c => c.Name)
                .NotEmpty()
                   .WithMessage("Name is required.")
                .Length(1,100)
                   .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(c => c.Description)
                .MaximumLength(500)
                   .WithMessage("Description must not exceed 500 characters.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .When(c => !string.IsNullOrEmpty(c.Email))
                   .WithMessage("A valid email address is required.");
                
        }
    }
}
