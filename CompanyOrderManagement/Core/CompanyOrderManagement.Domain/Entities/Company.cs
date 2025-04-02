using CompanyOrderManagement.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string? Email { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
