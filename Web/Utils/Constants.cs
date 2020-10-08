namespace Web.Utils
{
    internal static class Constants
    {
        internal static class CustomIdentityServerScopes
        {
            // Identity resource scopes
            internal const string Permission = "permission";

            // API resource scopes
            internal const string DeleteTodoItem = "delete.todoitem";
            internal const string ReadTodoItem = "read.todoitem";
            internal const string WorkerTodoItem = "worker.todoitem";
            internal const string WriteTodoItem = "write.todoitem";
        }

        internal static class PermissionNames
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
