using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the output mechanism of a legacy console.
    /// </summary>
    /// <remarks>
    /// The implementation is internally backed by System.Console.
    /// </remarks>
    public class SystemConsoleOutput : IConsoleOutput
    {
        private readonly ColorSystem _colorSystem;

        private Style _lastStyle = Style.Plain;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConsoleOutput"/> class.
        /// </summary>
        /// <param name="colorSystem">The consoles color system.</param>
        public SystemConsoleOutput(ColorSystem colorSystem)
        {
            _colorSystem = colorSystem;
        }

        /// <inheritdoc />
        public void Clear(bool home)
        {
            var (x, y) = (System.Console.CursorLeft, System.Console.CursorTop);

            System.Console.Clear();

            if (!home)
            {
                // Set the cursor position
                System.Console.SetCursorPosition(x, y);
            }
        }

        /// <inheritdoc />
        public void Flush()
        {
            System.Console.Out.Flush();
        }

        /// <inheritdoc />
        public void Write(Segment segment)
        {
            if (segment.IsControlCode)
            {
                return;
            }

            if (!_lastStyle.Equals(segment.Style))
            {
                SetStyle(segment.Style);
            }

            System.Console.Out.Write(segment.Text.NormalizeNewLines(native: true));
        }

        private void SetStyle(Style style)
        {
            _lastStyle = style;

            System.Console.ResetColor();

            var background = Color.ToConsoleColor(style.Background);
            if (_colorSystem != ColorSystem.NoColors && (int)background != -1)
            {
                System.Console.BackgroundColor = background;
            }

            var foreground = Color.ToConsoleColor(style.Foreground);
            if (_colorSystem != ColorSystem.NoColors && (int)foreground != -1)
            {
                System.Console.ForegroundColor = foreground;
            }
        }
    }
}
