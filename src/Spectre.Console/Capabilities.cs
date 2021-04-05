using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public class SystemCapabilities : ICapabilities
    {
        private static readonly Lazy<SystemCapabilities> DefaultEnv = new(FromDefaultEnvironment);
        private static readonly Lazy<AnsiDetectionResult> DetectedAnsi = new(() => AnsiDetector.Detect(true));

        private SystemCapabilities()
        {

        }

        public static SystemCapabilities DefaultEnvironment => DefaultEnv.Value;


        public static SystemCapabilities FromDefaultEnvironment()
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

            return FromEnvironment(variables);
        }

        public static SystemCapabilities FromEnvironment(IDictionary<string, string> variables)
        {
            if (variables is null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var ansiMode = DetectedAnsi.Value;
            var colorSystem = ColorSystemDetector.Detect()

            return new()
            {

            }
        }

        private static (bool Ansi, bool Legacy) DetectAnsi(AnsiSupport ansiSupport)
        {
            var supportsAnsi = ansiSupport == AnsiSupport.Yes;
            var legacyConsole = false;

            if (ansiSupport == AnsiSupport.Detect)
            {
                (supportsAnsi, legacyConsole) = AnsiDetector.Detect(true);
            }
            else
            {
                // Are we running on Windows?
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Not the first console we're creating?
                    if (AnsiConsole.Created)
                    {
                        legacyConsole = AnsiConsole.Profile.Capabilities.Legacy;
                    }
                    else
                    {
                        // Try detecting whether or not this is a legacy console
                        (_, legacyConsole) = AnsiDetector.Detect(false);
                    }
                }
            }

            return (supportsAnsi, legacyConsole);
        }

        public bool Ansi { get; init; }
        public bool Links { get; init; }
        public bool Legacy { get; init; }
        public bool Interactive { get; init; }
        public bool Unicode { get; init; }
        public ColorSystem ColorSystem { get; init; }
    }
}
