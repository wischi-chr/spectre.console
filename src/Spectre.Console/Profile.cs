using System;
using System.IO;

namespace Spectre.Console
{
    /// <summary>
    /// Represents an editable profile.
    /// </summary>
    public class Profile : IConsoleBackend
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        /// <param name="standardOutput">The standard output text writer.</param>
        public Profile(TextWriter standardOutput)
        {
            StandardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
        }

        /// <inheritdoc/>
        public virtual ICapabilities Capabilities { get; set; } = new Capabilities();

        /// <inheritdoc/>
        public virtual TextWriter StandardOutput { get; }

        /// <inheritdoc/>
        public virtual int Width { get; set; }

        /// <inheritdoc/>
        public virtual int Height { get; set; }

        /// <inheritdoc/>
        public virtual ConsoleKeyInfo ReadKey(bool intercept)
        {
            // default implementation just throws
            throw new InvalidOperationException();
        }
    }
}
