using CompanyOrderManagement.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int Stock {  get; set; }
        public int Price { get; set; }
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public Guid ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }

    }
}
