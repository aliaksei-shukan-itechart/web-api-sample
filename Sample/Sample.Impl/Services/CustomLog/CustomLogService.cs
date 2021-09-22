using System;

namespace Sample.Impl.Services.CustomLog
{
    public class CustomLogService : ICustomLogService
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"Custom Middleware Logs: {message}");
        }
    }
}
