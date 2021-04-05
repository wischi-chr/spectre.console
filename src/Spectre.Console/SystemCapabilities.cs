using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console.Enrichment;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the system console's capabilities.
    /// </summary>
    public static class SystemCapabilities
    {
        private static readonly Lazy<ICapabilities> _defaultEnv = new(FromEnvironment);
        private static readonly Lazy<AnsiDetectionResult> _detectedAnsi = new(() => AnsiDetector.Detect(true));

        /// <summary>
        /// Gets the default system console capabilities.
        /// </summary>
        /// <remarks>
        /// The capabilities are detected using the default enrichers and system environment variables.
        /// </remarks>
        public static ICapabilities Default => _defaultEnv.Value;

        /// <summary>
        /// Determines the system consoles capabilities
        /// and taking the environment variables into account.
        /// </summary>
        /// <returns>The capabilities of the system console.</returns>
        private static ICapabilities FromEnvironment()
        {
            var variables = Environment
                .GetEnvironmentVariables()
                .Cast<DictionaryEntry>()
                .Aggregate(
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                    (dictionary, entry) =>
                    {
                        var key = (string)entry.Key;
                        if (!dictionary.TryGetValue(key, out _))
                        {
                            dictionary.Add(key, entry.Value as string ?? string.Empty);
                        }

                        return dictionary;
                    },
                    dictionary => dictionary);

            return CustomDetect(variables).AsReadOnly();
        }

        /// <summary>
        /// Determines the system consoles capabilities
        /// and taking the given <paramref name="variables"/> into account.
        /// </summary>
        /// <remarks>
        /// Consider using <see cref="Default"/> to get the system console's capabilities.
        /// Use this method if you want to provide custom values for
        /// environment variables, enrichment or the console output <see cref="TextWriter"/>.
        /// </remarks>
        /// <param name="variables">
        /// Environment variables that are taken into account
        /// during detection and enrichment.
        /// </param>
        /// <param name="enrichment">Enrichment settings.</param>
        /// <param name="consoleOut">
        /// The console out writer used for unicode detection.
        /// Defaults to <see cref="System.Console.Out"/>.
        /// </param>
        /// <returns>The capabilities of the system console.</returns>
        public static Capabilities CustomDetect(
            IDictionary<string, string>? variables = null,
            CapabilitiesEnrichment? enrichment = null,
            TextWriter? consoleOut = null)
        {
            variables ??= new Dictionary<string, string>();
            enrichment ??= new CapabilitiesEnrichment();
            consoleOut ??= System.Console.Out;

            var ansiMode = _detectedAnsi.Value;
            var colorSystem = ColorSystemDetector.Detect(ansiMode.SupportsAnsi);

            var capabilities = new Capabilities()
            {
                Ansi = ansiMode.SupportsAnsi,
                ColorSystem = colorSystem,
                Interactive = Environment.UserInteractive,
                Legacy = ansiMode.LegacyConsole,
                Links = ansiMode.SupportsAnsi && !ansiMode.LegacyConsole,
                Unicode = consoleOut.Encoding.EncodingName.ContainsExact("Unicode"),
            };

            CapabilitiesEnricher.Enrich(
                capabilities,
                enrichment,
                variables);

            return capabilities;
        }
    }
}
