namespace Web.Utils
{
    internal static class Constants
    {
        internal class ConsentOptions
        {
            public static bool EnableOfflineAccess = true;
            public static string OfflineAccessDisplayName = "Offline Access";
            public static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

            public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";
            public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
        }

        internal class CustomIdentityServerScopes
        {
            // Identity resource scopes
            internal const string Permission = "permission";

            // API resource scopes
            internal const string DeleteTodoItem = "delete.todoitem";
            internal const string ReadTodoItem = "read.todoitem";
            internal const string WorkerTodoItem = "worker.todoitem";
            internal const string WriteTodoItem = "write.todoitem";
        }

        internal class PermissionNames
        {
            // Permission
            public const string CreatePermission = "Create Permission";
            public const string DeletePermission = "Delete Permission";
            public const string EditPermission = "Edit Permission";
            public const string ViewPermission = "View Permission";

            // Role
            public const string CreateRole = "Create Role";
            public const string DeleteRole = "Delete Role";
            public const string EditRole = "Edit Role";
            public const string ViewRole = "View Role";

            // TodoItem: permissions for admin operations on a todo-item microservice
            public const string ViewTodoItem = "View TodoItem";

            // User
            public const string CreateUser = "Create User";
            public const string DeleteUser = "Delete User";
            public const string EditUser = "Edit User";
            public const string ViewUser = "View User";
        }
    }
}
