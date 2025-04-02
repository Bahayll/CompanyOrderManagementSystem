using CompanyOrderManagement.Domain.Entities.Common;
using CompanyOrderManagement.Domain.Entities.Identity;
using CompanyOrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }
        public OrderStatus? OrderStatus { get; set; }   


        public ICollection<Product>? Products { get; set; } = new List<Product>();


        public Guid UserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
