using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Rules.Orders
{
    public interface IOrderBusinessRules
    {
        Task EnsureUserIsRegisteredForOrderAsync(Guid userId);
        Task EnsureCompanyExistsAsync(Guid companyId);
        Task EnsureOrderIdExists(Guid id);

    }
}
