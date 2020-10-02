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
    }
}
