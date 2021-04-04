using System;
using System.Runtime.InteropServices;
using Spectre.Console.Enrichment;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Factory for creating an ANSI console.
    /// </summary>
    public sealed class AnsiConsoleFactory
    {
        /// <summary>
        /// Creates an ANSI console.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>An implementation of <see cref="IAnsiConsole"/>.</returns>
        public IAnsiConsole Create(AnsiConsoleSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var buffer = settings.Out ?? throw new NotSupportedException();

            // Detect if the terminal support ANSI or not
            // var (supportsAnsi, legacyConsole) = DetectAnsi(settings, buffer);

            var supportsAnsi = true;
            var legacyConsole = false;

            // Get the color system
            var colorSystem = settings.ColorSystem == ColorSystemSupport.Detect
                ? ColorSystemDetector.Detect(supportsAnsi)
                : (ColorSystem)settings.ColorSystem;

            // Get whether or not we consider the terminal interactive
            var interactive = settings.Interactive == InteractionSupport.Yes;
            if (settings.Interactive == InteractionSupport.Detect)
            {
                interactive = Environment.UserInteractive;
            }

            var capabilities = new Capabilities()
            {
                ColorSystem = colorSystem,
                Ansi = supportsAnsi,
                Links = supportsAnsi && !legacyConsole,
                Legacy = legacyConsole,
                Interactive = interactive,
                Unicode = true, // TODO: Fix that
            };

            // Enrich the profile
            CapabilitiesEnricher.Enrich(
                capabilities,
                settings.Enrichment,
                settings.EnvironmentVariables);

            var profile = new Profile(buffer)
            {
                Capabilities = capabilities,
            };

            return new AnsiConsoleFacade(
                profile,
                settings.ExclusivityMode ?? new DefaultExclusivityMode());
        }
    }
}
