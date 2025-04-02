

namespace CompanyOrderManagement.Application.Logging.LogMessages.Companies
{
    public static class CompanyLogMessage
    {
        public static string SuccessfullyCreated(string Name) => $"Company with name {Name} has been successfully created.";
        public static string SuccessfullyDeleted(Guid CompanyId) => $"Company with ID {CompanyId} has been successfully deleted.";
        public static string SuccessfullyUpdated(Guid CompanyId) => $"Company with ID {CompanyId} has been successfully updated.";
        public static string SuccessfullyFetchedAll(int count) => $"Successfully fetched {count} companies.";
        public static string SuccessfullyFetchedById(Guid CompanyId) => $"Successfully fetched company with ID {CompanyId}.";

        public static string FailedToCreate(string exceptionMessage) => $"Failed to create company. Exception: {exceptionMessage}";
        public static string FailedToDelete(Guid CompanyId, string exceptionMessage) => $"Failed to delete company with ID {CompanyId}. Exception: {exceptionMessage}";
        public static string FailedToUpdate(Guid CompanyId, string exceptionMessage) => $"Failed to update company with ID {CompanyId}. Exception: {exceptionMessage}";
        public static string FailedToFetchAll(string exceptionMessage) => $"Failed to fetch all companies. Exception: {exceptionMessage}";
        public static string FailedToFetchById(Guid CompanyId, string exceptionMessage) => $"Failed to fetch company with ID {CompanyId}. Exception: {exceptionMessage}";


        public static string CacheUpdated(string cacheKey, string Name) => $"Cache updated for key: {cacheKey} with new company: {Name}";
        public static string CacheRemoved(Guid id) => $"Company with ID: {id} removed from cache and cache updated.";
        public static string CacheUpdatedForCompany(Guid companyId) => $"Company with ID: {companyId} updated in cache.";
        public static string CacheAddedForCompany(Guid companyId) => $"Company with ID: {companyId} added to cache.";
        public static string CacheRetrievedFromCache(int count) => $"Data retrieved from cache. Successfully fetched {count} companies.";
        public static string CacheNotFoundFetchingFromDatabase() => "Data not found in cache. Fetching from database.";
        public static string CacheSuccessfullyFetchedById(Guid companyId) => $"Company with ID: {companyId} successfully fetched from cache.";


    }
}
