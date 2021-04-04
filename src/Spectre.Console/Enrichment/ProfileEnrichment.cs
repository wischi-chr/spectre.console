using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Contains settings for profile enrichment.
    /// </summary>
    public sealed class CapabilitiesEnrichment

    {
        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// any default enrichers should be added.
        /// </summary>
        /// <remarks>Defaults to <c>true</c>.</remarks>
        public bool UseDefaultEnrichers { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of custom enrichers to use.
        /// </summary>
        public List<ICapabilitiesEnricher>? Enrichers { get; set; }
    }
}
