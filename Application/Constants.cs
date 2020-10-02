namespace Application
{
    public static class Constants
    {
        public static class PermissionNames
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
            internal const string ViewTodoItem = "View TodoItem";

            // User
            public const string CreateUser = "Create User";
            public const string DeleteUser = "Delete User";
            public const string EditUser = "Edit User";
            public const string ViewUser = "View User";
        }
    }
}
