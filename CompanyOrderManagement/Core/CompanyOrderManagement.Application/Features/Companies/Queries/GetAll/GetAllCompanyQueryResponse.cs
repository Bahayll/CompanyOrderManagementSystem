

namespace CompanyOrderManagement.Application.Features.Companies.Queries.GetAll
{
    public class GetAllCompanyQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
