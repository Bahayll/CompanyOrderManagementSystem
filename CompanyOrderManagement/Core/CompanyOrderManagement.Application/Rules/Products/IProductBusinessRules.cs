using CompanyOrderManagement.Application.Features.Products.Commands.Update;


namespace CompanyOrderManagement.Application.Rules.Products
{
    public interface IProductBusinessRules
    {
        Task EnsureUniqueProductNameAsync(string name);
        Task EnsureCategoryExistsAsync(Guid categoryId);
        Task EnsureCompanyExistsAsync(Guid companyId);
        Task EnsureProductIdExists(Guid id);
        Task EnsureProductDetailsAreUpdated(UpdateProductCommandRequest request);
        Task ProductNameCanNotBeDuplicatedWhenUpdated(string name, Guid id);
    }
}
