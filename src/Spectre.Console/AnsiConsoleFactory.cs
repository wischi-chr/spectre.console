using System;

namespace Spectre.Console
{
    /// <summary>
    /// Factory for creating an ANSI console.
    /// </summary>
    public sealed class AnsiConsoleFactory
    {
        /// <summary>
        /// Creates an ANSI console.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>An implementation of <see cref="IAnsiConsole"/>.</returns>
        public IAnsiConsole Create(AnsiConsoleSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
