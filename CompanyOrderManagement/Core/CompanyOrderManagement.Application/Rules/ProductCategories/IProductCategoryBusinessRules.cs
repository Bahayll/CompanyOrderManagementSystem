

using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update;

namespace CompanyOrderManagement.Application.Rules.ProductCategories
{
    public interface IProductCategoryBusinessRules
    {
        Task EnsureUniqueCategoryNameAsync(string name);
        Task EnsureProductCategoryIdExists(Guid id);
        Task ProductCategoryNameCanNotBeDuplicatedWhenUpdated(string name, Guid id);
        Task EnsureProductDetailsAreUpdated(UpdateProductCategoryCommandRequest request);
    }
}
