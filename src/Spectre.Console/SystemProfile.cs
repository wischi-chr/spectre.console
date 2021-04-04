using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the system console.
    /// </summary>
    public partial class SystemProfile : IConsoleBackend, ICapabilities
    {
        internal SystemProfile()
        {
            StandardOutput = System.Console.Out;
            Capabilities = this;
        }

        /// <summary>
        /// Gets the singleton instance of the system console.
        /// </summary>
        public static SystemProfile Instance { get; }
            = new SystemProfile();

        /// <inheritdoc/>
        public TextWriter StandardOutput { get; internal set; }

        /// <inheritdoc/>
        public int Width
        {
            get => ConsoleHelper.GetSafeWidth();
            set
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    System.Console.WindowWidth = value;
                }
            }
        }

        /// <inheritdoc/>
        public int Height
        {
            get => ConsoleHelper.GetSafeHeight();
            set
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    System.Console.WindowHeight = value;
                }
            }
        }

        /// <inheritdoc/>
        public ICapabilities Capabilities { get; internal set; }

        /// <inheritdoc/>
        bool ICapabilities.Ansi { get; } = true;

        /// <inheritdoc/>
        bool ICapabilities.Links { get; } = true;

        /// <inheritdoc/>
        bool ICapabilities.Legacy { get; } = false;

        /// <inheritdoc/>
        bool ICapabilities.Interactive { get; } = true;

        /// <inheritdoc/>
        bool ICapabilities.Unicode { get; } = true;

        /// <inheritdoc/>
        ColorSystem ICapabilities.ColorSystem { get; } = ColorSystem.TrueColor;

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return System.Console.ReadKey(intercept);
        }
    }

    public partial class SystemProfile
    {
        private static bool upgraded;

        static SystemProfile()
        {

        }

        public static bool Ansi { get; }
        public static bool Legacy { get; }

        public static void Upgrade()
        {

        }

        private static (bool Ansi, bool Legacy) DetectAnsi(AnsiSupport ansiSupport)
        {
            var supportsAnsi = ansiSupport == AnsiSupport.Yes;
            var legacyConsole = false;

            if (ansiSupport == AnsiSupport.Detect)
            {
                (supportsAnsi, legacyConsole) = AnsiDetector.Detect(true);

                // Check whether or not this is a legacy console from the existing instance (if any).
                // We need to do this because once we upgrade the console to support ENABLE_VIRTUAL_TERMINAL_PROCESSING
                // on Windows, there is no way of detecting whether or not we're running on a legacy console or not.
                if (AnsiConsole.Created && !legacyConsole && AnsiConsole.Profile.Capabilities.Legacy)
                {
                    legacyConsole = AnsiConsole.Profile.Capabilities.Legacy;
                }
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
    }
}
