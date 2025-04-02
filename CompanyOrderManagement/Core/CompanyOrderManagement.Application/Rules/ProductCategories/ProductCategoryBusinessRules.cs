using CompanyOrderManagement.Domain.Exceptions;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update;
using CompanyOrderManagement.Application.Features.ProductCategories.ConstantMessages;

namespace CompanyOrderManagement.Application.Rules.ProductCategories
{
    public class ProductCategoryBusinessRules : IProductCategoryBusinessRules
    {
        private readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;

        public ProductCategoryBusinessRules(IProductCategoryUnitOfWork productCategoryUnitOfWork)
        {
            _productCategoryUnitOfWork = productCategoryUnitOfWork;
        }

        public async Task EnsureProductDetailsAreUpdated(UpdateProductCategoryCommandRequest request)
        {
            var detailcategory = await _productCategoryUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
            if (detailcategory != null && detailcategory.Name == request.Name && detailcategory.Description == request.Description)
                throw new BusinessException(ProductCategoryMessages.ProductCategoryDetailsUpToDate);
            
        }

        public async Task EnsureProductCategoryIdExists(Guid id)
        {
            var productCategory = await _productCategoryUnitOfWork.GetReadRepository.GetByIdAsync(id);
            if (productCategory == null)
                throw new BusinessException(ProductCategoryMessages.ProductCategoryNotFound);

        }

        public async Task EnsureUniqueCategoryNameAsync(string name)
        {
            var CategoryName = await _productCategoryUnitOfWork.GetReadRepository.GetAll().FirstOrDefaultAsync(p => p.Name == name);
            if (CategoryName != null)
                throw new BusinessException(ProductCategoryMessages.CategoryNameExists);

        }
        public async Task ProductCategoryNameCanNotBeDuplicatedWhenUpdated(string name, Guid id)
        {
            var existingCategory = await _productCategoryUnitOfWork.GetReadRepository.GetAll().FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != id);
            if (existingCategory != null)
                throw new BusinessException(ProductCategoryMessages.ProductCategoryNameExists);
        }

        }
    }


