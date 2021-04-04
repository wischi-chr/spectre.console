namespace Spectre.Console
{
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
    }
}
