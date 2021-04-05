using System.IO;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the interface to a console.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Gets the capabilities of the console.
        /// </summary>
        ICapabilities Capabilities { get; }

        /// <summary>
        /// Gets the console cursor for the backend.
        /// </summary>
        IConsoleCursor Cursor { get; }

        /// <summary>
        /// Gets the console's output mechanism.
        /// </summary>
        IConsoleOutput Output { get; }

        /// <summary>
        /// Gets the console's input mechanism.
        /// </summary>
        IConsoleInput Input { get; }

        /// <summary>
        /// Gets the console's window mechanism.
        /// </summary>
        IConsoleWindow Window { get; }
    }

    /// <summary>
    /// Represents the console's input mechanism.
    /// </summary>
    public interface IConsoleOutput
    {
        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="home">If the cursor should be moved to the home position.</param>
        void Clear(bool home);

        /// <summary>
        /// Writes a segment to the console.
        /// </summary>
        /// <param name="segment">The segment that is written.</param>
        void Write(Segment segment);

        /// <summary>
        /// Flushes the output buffer.
        /// </summary>
        void Flush();
    }

    /// <summary>
    /// Represents the console's window mechanism.
    /// </summary>
    public interface IConsoleWindow
    {
        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the window width.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the window height.
        /// </summary>
        int Height { get; set; }
    }
}
