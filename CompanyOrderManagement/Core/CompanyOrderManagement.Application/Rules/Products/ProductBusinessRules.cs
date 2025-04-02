using CompanyOrderManagement.Domain.Exceptions;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using Microsoft.EntityFrameworkCore;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Application.Features.Products.Commands.Update;
using CompanyOrderManagement.Application.Features.ProductCategories.ConstantMessages;
using CompanyOrderManagement.Application.Features.Products.ConstantMessages;

namespace CompanyOrderManagement.Application.Rules.Products
{
    public class ProductBusinessRules : IProductBusinessRules
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;

        public ProductBusinessRules(IProductUnitOfWork productUnitOfWork, ICompanyUnitOfWork companyUnitOfWork, IProductCategoryUnitOfWork productCategoryUnitOfWork)
        {
            _productUnitOfWork = productUnitOfWork;
            _companyUnitOfWork = companyUnitOfWork;
            _productCategoryUnitOfWork = productCategoryUnitOfWork;
        }

        public async Task EnsureCategoryExistsAsync(Guid categoryId)
        {
            var ProductCategory = await _productCategoryUnitOfWork.GetReadRepository.GetByIdAsync(categoryId);
            if (ProductCategory == null)
                throw new BusinessException(ProductMessages.ProductCategoryNotFound);

        }

        public async Task EnsureCompanyExistsAsync(Guid companyId)
        {
            var company = await _companyUnitOfWork.GetReadRepository.GetByIdAsync(companyId);
            if (company == null)
                throw new BusinessException(ProductMessages.CompanyNotFound);
        }

        public async Task EnsureProductDetailsAreUpdated(UpdateProductCommandRequest request)
        {
            var productDetail = await _productUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
            if (productDetail != null && productDetail.Stock == request.Stock && productDetail.Price == request.Price &&
                productDetail.Description == request.Description && productDetail.CompanyId == request.CompanyId &&
                productDetail.ProductCategoryId == request.ProductCategoryId)
                throw new BusinessException(ProductMessages.ProductDetailsUpToDate);
        }

        public async Task EnsureProductIdExists(Guid id)
        {
            var product = await _productUnitOfWork.GetReadRepository.GetByIdAsync(id);
            if (product == null)
                throw new BusinessException(ProductMessages.ProductNotFound);
        }

        public async Task EnsureUniqueProductNameAsync(string name)
        {
            var product = await _productUnitOfWork.GetReadRepository.GetAll().FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (product != null)
                throw new BusinessException(ProductMessages.ProductNameExists);
        }

        public async Task ProductNameCanNotBeDuplicatedWhenUpdated(string name, Guid id)
        {
            var existingProduct = await _productUnitOfWork.GetReadRepository.GetAll().FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != id);
            if (existingProduct != null)
                throw new BusinessException(ProductMessages.ProductAlreadyExists);
        }
    }
}
