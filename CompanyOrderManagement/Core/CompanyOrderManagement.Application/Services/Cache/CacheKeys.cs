using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Services.Cache
{
    public static class CacheKeys
    {
        public const string AllCompanies = "AllCompanies";
        public const string GetAllCompanies = "GetAllCompanies";
        public static string CompanyById(Guid id) => $"CompanyById_{id}";


        public const string AllProductCategories = "AllProductCategories";
        public const string GetAllProductCategories = "GetAllProductCategories";
        public static string ProductCategoryById(Guid id) => $"ProductCategoryById_{id}";
    }
}
