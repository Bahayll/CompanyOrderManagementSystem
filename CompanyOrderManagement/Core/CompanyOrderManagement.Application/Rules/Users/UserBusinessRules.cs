using CompanyOrderManagement.Application.Features.AppUsers.Commands.Update;
using CompanyOrderManagement.Application.Features.AppUsers.ConstantMessages;
using CompanyOrderManagement.Domain.Entities.Identity;
using CompanyOrderManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace CompanyOrderManagement.Application.Rules.Users
{
    public class UserBusinessRules : IUserBusinessRules
    {
        private readonly UserManager<AppUser> _userManager;

        public UserBusinessRules(UserManager<AppUser> manager)
        {
            _userManager = manager;
        }

        public async Task EnsureUserIdExists(Guid id)
        {
           
            var company = await _userManager.FindByIdAsync(id.ToString());
            if (company == null)
                throw new BusinessException(AppUserMessages.UserNotFound);
            
        }
        public async Task EnsureUserDetailsAreUpdated(UpdateAppUserCommandRequest request)
        {
            var userDetail = await _userManager.FindByIdAsync(request.Id);
            if (userDetail != null && request.Email == userDetail.Email && userDetail.UserName == request.UserName && request.FullName == userDetail.FullName
                )
                throw new BusinessException(AppUserMessages.UserDetailsUpToDate);
 
        }

        public void EnsurePasswordsMatch(string password, string passwordConfirm)
        {
            if (password != passwordConfirm)
                throw new BusinessException(AppUserMessages.PasswordsDoNotMatch);
        }

        public async Task EnsureUserExistsByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail) ?? await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null)
                throw new NotFoundUserException();
        }

        public async Task EnsurePasswordIsValidAsync(AppUser user, string password)
        {
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
                throw new AuthenticationErrorException();

        }
    }
}
