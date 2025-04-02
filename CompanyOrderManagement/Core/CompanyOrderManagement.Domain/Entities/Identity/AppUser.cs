using Microsoft.AspNetCore.Identity;

namespace CompanyOrderManagement.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedDate { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
