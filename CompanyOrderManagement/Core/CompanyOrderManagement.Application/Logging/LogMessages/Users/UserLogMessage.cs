
namespace CompanyOrderManagement.Application.Logging.LogMessages.Users
{
    public static class UserLogMessage
    {
        public static string SuccessfullyCreated(string Name) => $"User with name {Name} has been successfully created.";
        public static string SuccessfullyDeleted(Guid UserId) => $"User with ID {UserId} has been successfully deleted.";
        public static string SuccessfullyUpdated(Guid UserId) => $"User with ID {UserId} has been successfully updated.";
        public static string SuccessfullyFetchedAll(int count) => $"Successfully fetched {count} users.";
        public static string SuccessfullyFetchedById(Guid UserId) => $"Successfully fetched User with ID {UserId}.";

        public static string FailedToCreate(string exceptionMessage) => $"Failed to create User. Exception: {exceptionMessage}";
        public static string FailedToDelete(Guid UserId, string exceptionMessage) => $"Failed to delete User with ID {UserId}. Exception: {exceptionMessage}";
        public static string FailedToUpdate(Guid UserId, string exceptionMessage) => $"Failed to update User with ID {UserId}. Exception: {exceptionMessage}";
        public static string FailedToFetchAll(string exceptionMessage) => $"Failed to fetch all users. Exception: {exceptionMessage}";
        public static string FailedToFetchById(Guid UserId, string exceptionMessage) => $"Failed to fetch User with ID {UserId}. Exception: {exceptionMessage}";
    }
}
