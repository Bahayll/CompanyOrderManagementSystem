

namespace CompanyOrderManagement.Application.Logging.LogMessages.Orders
{
    public static class OrderLogMessage
    {
        public static string SuccessfullyCreated(string Name) => $"Order with ID {Name} has been successfully created.";
        public static string SuccessfullyDeleted(Guid orderId) => $"Order with ID {orderId} has been successfully deleted.";
        public static string SuccessfullyUpdated(Guid orderId) => $"Order with ID {orderId} has been successfully updated.";
        public static string SuccessfullyFetchedAll(int count) => $"Successfully fetched {count} orders.";
        public static string SuccessfullyFetchedById(Guid orderId) => $"Successfully fetched order with ID {orderId}.";

        public static string FailedToCreate(string exceptionMessage) => $"Failed to create order. Exception: {exceptionMessage}";
        public static string FailedToDelete(Guid orderId, string exceptionMessage) => $"Failed to delete order with ID {orderId}. Exception: {exceptionMessage}";
        public static string FailedToUpdateO(Guid orderId, string exceptionMessage) => $"Failed to update order with ID {orderId}. Exception: {exceptionMessage}";
        public static string FailedToFetchAll(string exceptionMessage) => $"Failed to fetch all orders. Exception: {exceptionMessage}";
        public static string FailedToFetchById(Guid orderId, string exceptionMessage) => $"Failed to fetch order with ID {orderId}. Exception: {exceptionMessage}";

    }
}
