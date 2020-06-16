using Microsoft.Extensions.Logging;

namespace Wikiled.Common.Logging
{
    public class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();

        public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName); 
    }
}
