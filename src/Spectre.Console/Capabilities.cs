namespace Spectre.Console
{
    /// <summary>
    /// Represents editable capabilities.
    /// </summary>
    public class Capabilities : ICapabilities
    {
        /// <inheritdoc/>
        public bool Ansi { get; set; } = true;

        /// <inheritdoc/>
        public bool Links { get; set; } = true;

        /// <inheritdoc/>
        public bool Legacy { get; set; }

        /// <inheritdoc/>
        public bool Interactive { get; set; }

        /// <inheritdoc/>
        public bool Unicode { get; set; } = true;

        /// <inheritdoc/>
        public ColorSystem ColorSystem { get; set; } = ColorSystem.TrueColor;
    }
}
