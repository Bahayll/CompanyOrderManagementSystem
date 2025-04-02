using CompanyOrderManagement.Domain.Exceptions;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.Features.Orders.ConstantMessages;
using Microsoft.AspNetCore.Identity;
using CompanyOrderManagement.Domain.Entities.Identity;


namespace CompanyOrderManagement.Application.Rules.Orders
{
    public class OrderBusinessRules : IOrderBusinessRules
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public OrderBusinessRules(IOrderUnitOfWork orderUnitOfWork, ICompanyUnitOfWork companyUnitOfWork, UserManager<AppUser> userManager)
        {

            _orderUnitOfWork = orderUnitOfWork;
            _companyUnitOfWork = companyUnitOfWork;
            _userManager = userManager;
        }

        public async Task EnsureCompanyExistsAsync(Guid companyId)
        {
            var company = await _companyUnitOfWork.GetReadRepository.GetByIdAsync(companyId);
            if (company == null)
                throw new BusinessException(OrderMessages.CompanyMustExistForOrder);
        }

        public async Task EnsureOrderIdExists(Guid id)
        {
            var order = await _orderUnitOfWork.GetReadRepository.GetByIdAsync(id);
            if (order == null)
                throw new BusinessException(OrderMessages.OrderIdNotFound);
        }

        public async Task EnsureUserIsRegisteredForOrderAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new BusinessException(OrderMessages.UserMustBeRegisteredForOrder);
               
            
        }
   
    }
}
