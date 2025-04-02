using CompanyOrderManagement.Application.Features.AppUsers.Commands.Update;
using CompanyOrderManagement.Domain.Entities.Identity;


namespace CompanyOrderManagement.Application.Rules.Users
{
    public interface IUserBusinessRules
    {
        Task EnsureUserIdExists(Guid id);
        Task EnsureUserDetailsAreUpdated(UpdateAppUserCommandRequest request);
        void EnsurePasswordsMatch(string password, string passwordConfirm);
        Task EnsureUserExistsByUsernameOrEmailAsync(string usernameOrEmail);
        Task EnsurePasswordIsValidAsync(AppUser user, string password);

    }
}
