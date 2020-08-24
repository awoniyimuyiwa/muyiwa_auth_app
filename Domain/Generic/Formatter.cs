using System;

namespace Domain
{
    public static class Formatter
    {
        public static string Format(DateTime dateTime)
        {
            return dateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }
    }
}
