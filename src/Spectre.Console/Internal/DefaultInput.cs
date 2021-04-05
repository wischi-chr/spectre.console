using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the console's input mechanism for System.Console.
    /// </summary>
    public sealed class SystemConsoleInput : IConsoleInput
    {
        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return System.Console.ReadKey(intercept);
        }
    }
}
