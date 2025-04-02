using CompanyOrderManagement.Application.Features.AppUsers.Commands.Update;
using FluentValidation;


namespace CompanyOrderManagement.Application.Validators.Users
{
    public class UpdateUserCommandRequestValidator : AbstractValidator<UpdateAppUserCommandRequest>
    {
        public UpdateUserCommandRequestValidator() 
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                   .WithMessage("User Id is required.");

            RuleFor(u => u.FullName)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Name is required")
                .Length(1, 100)
                     .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(u => u.UserName)
               .NotEmpty()
               .NotNull()
                   .WithMessage("UserName is required")
               .Length(1, 25)
                    .WithMessage("UserName must be between 1 and 25 characters.");

            RuleFor(u => u.Email)
                .EmailAddress()
                .When(u => !string.IsNullOrEmpty(u.Email))
                   .WithMessage("Invalid email format.");

            RuleFor(u => u.PhoneNumber)
                .Matches(@"^\+905\d{9}$")
                .WithMessage("Phone number must be in the format +905XXXXXXXXX where X is a digit.");

        }
    }
}
