using System;
using System.IO;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the interface to a console.
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        /// Gets the capabilities of the console.
        /// </summary>
        ICapabilities Capabilities { get; }

        /// <summary>
        /// Gets the standard output text writer.
        /// </summary>
        TextWriter StandardOutput { get; }

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>An object that describes the ConsoleKey constant and Unicode character, if any, that correspond to the pressed console key.</returns>
        ConsoleKeyInfo ReadKey(bool intercept);

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
