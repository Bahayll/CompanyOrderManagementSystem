

namespace CompanyOrderManagement.Application.Logging.LogMessages.Products
{
    public static class ProductLogMessage
    {
        public static string SuccessfullyCreated(string Name) => $"Product with name {Name} has been successfully created.";
        public static string SuccessfullyDeleted(Guid ProductId) => $"Product with ID {ProductId} has been successfully deleted.";
        public static string SuccessfullyUpdated(Guid ProductId) => $"Product with ID {ProductId} has been successfully updated.";
        public static string SuccessfullyFetchedAll(int count) => $"Successfully fetched {count} products.";
        public static string SuccessfullyFetchedById(Guid ProductId) => $"Successfully fetched Product with ID {ProductId}.";

        public static string FailedToCreate(string exceptionMessage) => $"Failed to create Product. Exception: {exceptionMessage}";
        public static string FailedToDelete(Guid ProductId, string exceptionMessage) => $"Failed to delete Product with ID {ProductId}. Exception: {exceptionMessage}";
        public static string FailedToUpdate(Guid ProductId, string exceptionMessage) => $"Failed to update Product with ID {ProductId}. Exception: {exceptionMessage}";
        public static string FailedToFetchAll(string exceptionMessage) => $"Failed to fetch all products. Exception: {exceptionMessage}";
        public static string FailedToFetchById(Guid ProductId, string exceptionMessage) => $"Failed to fetch Product with ID {ProductId}. Exception: {exceptionMessage}";
    }
}
