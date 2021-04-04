using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console.Rendering;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleBackend : IConsoleBackend
    {


        public IConsoleCursor Cursor { get; }
        public ICapabilities Capabilities { get; }
        public IConsoleOutput Output { get; }
        public IConsoleInput Input { get; }

        public AnsiConsoleBackend(ICapabilities capabilities, IConsoleInput input, TextWriter standardOutput)
        {


            Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            Input = input ?? throw new ArgumentNullException(nameof(input));

            Cursor = new AnsiConsoleCursor(this);
            Output = new AnsiConsoleOutput(standardOutput);
        }
    }

    internal class Mist
    {
        public void Write(IRenderable renderable)
        {
            var builder = new StringBuilder();

            foreach (var segment in _console.GetSegments(renderable))
            {
                if (segment.IsControlCode)
                {
                    builder.Append(segment.Text);
                    continue;
                }

                var parts = segment.Text.NormalizeNewLines().Split(new[] { '\n' });
                foreach (var (_, _, last, part) in parts.Enumerate())
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        builder.Append(_builder.GetAnsi(part, segment.Style));
                    }

                    if (!last)
                    {
                        builder.Append(Environment.NewLine);
                    }
                }
            }

            if (builder.Length > 0)
            {
                _console.Output.StandardOutput.Write(builder.ToString());
                _console.Output.StandardOutput.Flush();
            }
        }
    }

    /// <summary>
    /// Represents the output mechanism of a legacy console.
    /// </summary>
    /// <remarks>
    /// The implementation is internally backed by System.Console.
    /// </remarks>
    public class LegacyConsoleOutput : IConsoleOutput
    {
        private readonly ColorSystem _colorSystem;

        private Style _lastStyle = Style.Plain;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyConsoleOutput"/> class.
        /// </summary>
        /// <param name="colorSystem">The consoles color system.</param>
        public LegacyConsoleOutput(ColorSystem colorSystem)
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

    /// <summary>
    /// Represents a virtual console window without a backend.
    /// </summary>
    public class ConsoleWindow : IConsoleWindow
    {
        /// <inheritdoc />
        public string Title { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Width { get; set; }

        /// <inheritdoc />
        public int Height { get; set; }
    }

    /// <summary>
    /// Represents the system console window.
    /// </summary>
    public class SystemConsoleWindow : IConsoleWindow
    {
        /// <inheritdoc />
        public string Title
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return System.Console.Title;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => System.Console.Title = value;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
    }

    /// <summary>
    /// Represents the output part of an <see cref="AnsiConsoleBackend"/>.
    /// </summary>
    public class AnsiConsoleOutput : IConsoleOutput
    {
        private readonly TextWriter _standardOutput;
        private readonly AnsiBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiConsoleOutput"/> class.
        /// </summary>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="capabilities">The console capabilities.</param>
        public AnsiConsoleOutput(TextWriter standardOutput, ICapabilities capabilities)
        {
            _standardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
            _builder = new AnsiBuilder(capabilities);
        }

        /// <inheritdoc />
        public void Clear(bool home)
        {
            _standardOutput.Write(ED(2));
            _standardOutput.Write(ED(3));

            if (home)
            {
                _standardOutput.Write(CUP(1, 1));
            }

            _standardOutput.Flush();
        }

        /// <inheritdoc />
        public void Flush()
        {
            _standardOutput.Flush();
        }

        /// <inheritdoc />
        public void Write(Segment segment)
        {
            if (segment.IsControlCode)
            {
                _standardOutput.Write(segment.Text);
                return;
            }

            var parts = segment.Text.NormalizeNewLines().Split(new[] { '\n' });
            foreach (var (_, _, last, part) in parts.Enumerate())
            {
                if (!string.IsNullOrEmpty(part))
                {
                    _standardOutput.Write(_builder.GetAnsi(part, segment.Style));
                }

                if (!last)
                {
                    _standardOutput.Write(Environment.NewLine);
                }
            }
        }
    }
}
