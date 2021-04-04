namespace Spectre.Console
{
    /// <summary>
    /// Represents the capabilites of an <see cref="IProfile"/>.
    /// </summary>
    public interface ICapabilities
    {
        /// <summary>
        /// Gets a value indicating whether or not
        /// the console supports Ansi.
        /// </summary>
        bool Ansi { get; }

        /// <summary>
        /// Gets a value indicating whether or not
        /// the console support links.
        /// </summary>
        bool Links { get; }

        /// <summary>
        /// Gets a value indicating whether or not
        /// this is a legacy console (cmd.exe) on an OS
        /// prior to Windows 10.
        /// </summary>
        /// <remarks>
        /// Only relevant when running on Microsoft Windows.
        /// </remarks>
        bool Legacy { get; }

        /// <summary>
        /// Gets a value indicating whether
        /// or not the console supports interaction.
        /// </summary>
        bool Interactive { get; }

        /// <summary>
        /// Gets a value indicating whether
        /// or not the console supports Unicode.
        /// </summary>
        bool Unicode { get; }

        /// <summary>
        /// Gets highest color system the console supports.
        /// </summary>
        public ColorSystem ColorSystem { get; }
    }
}
