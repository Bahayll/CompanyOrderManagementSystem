using CompanyOrderManagement.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public ICollection<Product>? Products { get; set; }
    }
}
