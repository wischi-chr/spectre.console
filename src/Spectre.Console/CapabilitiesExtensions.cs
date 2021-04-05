namespace Spectre.Console
{
    /// <summary>
    /// Extensions for <see cref="ICapabilities"/> and <see cref="Capabilities"/>.
    /// </summary>
    public static class CapabilitiesExtensions
    {
        /// <summary>
        /// Gets a value indicating whether or not a given <paramref name="colorSystem"/> is supported.
        /// </summary>
        /// <param name="capabilities">The capabilities the request is checked against.</param>
        /// <param name="colorSystem">The requested color system.</param>
        /// <returns>
        /// Return true if the requested <paramref name="colorSystem"/>
        /// is supported and false otherwise.
        /// </returns>
        public static bool Supports(this ICapabilities capabilities, ColorSystem colorSystem)
        {
            return colorSystem <= capabilities.ColorSystem;
        }

        /// <summary>
        /// Returns a readonly clone of the given capabilities.
        /// </summary>
        /// <param name="capabilities">The capabilities.</param>
        /// <returns>The readonly instance.</returns>
        public static ICapabilities AsReadOnly(this Capabilities capabilities)
        {
            return new ReadonlyCapabilities()
            {
                Ansi = capabilities.Ansi,
                ColorSystem = capabilities.ColorSystem,
                Interactive = capabilities.Interactive,
                Legacy = capabilities.Legacy,
                Links = capabilities.Links,
                Unicode = capabilities.Unicode,
            };
        }

        /// <summary>
        /// A private <see cref="ICapabilities"/> class to prevent edits through casting.
        /// </summary>
        private class ReadonlyCapabilities : ICapabilities
        {
            public bool Ansi { get; init; }
            public bool Links { get; init; }
            public bool Legacy { get; init; }
            public bool Interactive { get; init; }
            public bool Unicode { get; init; }
            public ColorSystem ColorSystem { get; init; }
        }
    }
}
