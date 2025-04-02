

namespace CompanyOrderManagement.Application.Logging.LogMessages.ProductCategories
{
    public static class ProductCategoryLogMessage
    {
        public static string SuccessfullyCreated(string Name) => $"ProductCategory with name {Name} has been successfully created.";
        public static string SuccessfullyDeleted(Guid ProductCategoryId) => $"ProductCategory with ID {ProductCategoryId} has been successfully deleted.";
        public static string SuccessfullyUpdated(Guid ProductCategoryId) => $"ProductCategory with ID {ProductCategoryId} has been successfully updated.";
        public static string SuccessfullyFetchedAll(int count) => $"Successfully fetched {count} productcategories.";
        public static string SuccessfullyFetchedById(Guid ProductCategoryId) => $"Successfully fetched ProductCategory with ID {ProductCategoryId}.";

        public static string FailedToCreate(string exceptionMessage) => $"Failed to create ProductCategory. Exception: {exceptionMessage}";
        public static string FailedToDelete(Guid ProductCategoryId, string exceptionMessage) => $"Failed to delete ProductCategory with ID {ProductCategoryId}. Exception: {exceptionMessage}";
        public static string FailedToUpdate(Guid ProductCategoryId, string exceptionMessage) => $"Failed to update ProductCategory with ID {ProductCategoryId}. Exception: {exceptionMessage}";
        public static string FailedToFetchAll(string exceptionMessage) => $"Failed to fetch all productcategories. Exception: {exceptionMessage}";
        public static string FailedToFetchById(Guid ProductCategoryId, string exceptionMessage) => $"Failed to fetch ProductCategory with ID {ProductCategoryId}. Exception: {exceptionMessage}";



        public static string CacheUpdated(string cacheKey, string Name) => $"Cache updated for key: {cacheKey} with new ProductCategory: {Name}";
        public static string CacheRemoved(Guid id) => $"ProductCategory with ID: {id} removed from cache and cache updated.";
        public static string CacheUpdatedForCategory(Guid companyId) => $"ProductCategory with ID: {companyId} updated in cache.";
        public static string CacheAddedForCategory(Guid companyId) => $"ProductCategory with ID: {companyId} added to cache.";
        public static string CacheRetrievedFromCache(int count) => $"Data retrieved from cache. Successfully fetched {count} productCategories.";
        public static string CacheNotFoundFetchingFromDatabase() => "Data not found in cache. Fetching from database.";
        public static string CacheSuccessfullyFetchedById(Guid companyId) => $"ProductCategory ID: {companyId} successfully fetched from cache.";
    }
}
