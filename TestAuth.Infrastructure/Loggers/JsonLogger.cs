using Microsoft.Extensions.Logging;

namespace TestAuth.Infrastructure.Loggers
{
    public sealed class JsonLogger : ILogger
    {
        private static readonly object ConsoleWriterLock = new object();
        private readonly string _name;
        private readonly Dictionary<LogLevel, ConsoleColor> _levelColors;

        public JsonLogger(string name)
        {
            _name = name;
            _levelColors = new Dictionary<LogLevel, ConsoleColor>
            {
                { LogLevel.Trace, ConsoleColor.Green },
                { LogLevel.Debug, ConsoleColor.Green },
                { LogLevel.Information, ConsoleColor.Green },
                { LogLevel.Warning, ConsoleColor.Yellow },
                { LogLevel.Error, ConsoleColor.Red },
                { LogLevel.Critical, ConsoleColor.Red },
                { LogLevel.None, ConsoleColor.Green }
            };
        }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            lock (ConsoleWriterLock)
            {
                var originalColor = Console.ForegroundColor;
                Console.Write("{");

                Console.ForegroundColor = _levelColors[logLevel];
                Console.Write($"\"dt\": \"{DateTime.UtcNow}\", ");
                Console.Write($"\"lvl\": \"{logLevel}\", ");

                Console.ForegroundColor = originalColor;
                Console.Write($"\"mess\": \"{GetLogMessgage(state, exception)}\", ");

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($"\"name\": \"{_name}\"");

                Console.ForegroundColor = originalColor;
                Console.Write("}");
                Console.WriteLine();
            }
        }

        private string? GetLogMessgage<TState>(TState state, Exception? exception)
        {
            if (exception != null)
            {
                return exception.ToString();
            }

            return state?.ToString();
        }
    }
}