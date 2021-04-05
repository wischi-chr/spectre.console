using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    public sealed class ConsoleBuildSettings
    {
        /// <summary>
        /// Gets or sets the desired capabilities.
        /// </summary>
        /// <remarks>
        /// Depending on the system the capabilities might be downgraded
        /// </remarks>
        public Capabilities DesiredCapabilities { get; set; } = new();

        /// <summary>
        /// Gets or sets the exclusivity mode.
        /// </summary>
        public IExclusivityMode? ExclusivityMode { get; set; }
    }

    /// <summary>
    /// A backend console.
    /// </summary>
    public class CustomBackendConsole : IConsole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBackendConsole"/> class.
        /// </summary>
        /// <param name="capabilities">.</param>
        /// <param name="cursor">.</param>
        /// <param name="output">.</param>
        /// <param name="input">.</param>
        /// <param name="window">.</param>
        public CustomBackendConsole(
            ICapabilities capabilities,
            IConsoleCursor cursor,
            IConsoleOutput output,
            IConsoleInput input,
            IConsoleWindow window)
        {
            Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            Cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            Output = output ?? throw new ArgumentNullException(nameof(output));
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Window = window ?? throw new ArgumentNullException(nameof(window));
        }

        /// <inheritdoc />
        public ICapabilities Capabilities { get; }

        /// <inheritdoc />
        public IConsoleCursor Cursor { get; }

        /// <inheritdoc />
        public IConsoleOutput Output { get; }

        /// <inheritdoc />
        public IConsoleInput Input { get; }

        /// <inheritdoc />
        public IConsoleWindow Window { get; }
    }

    /// <summary>
    /// Console Facade.
    /// </summary>
    public class ConsoleFacade : IAnsiConsole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleFacade"/> class.
        /// </summary>
        /// <param name="console">The backing console.</param>
        public ConsoleFacade(IConsole console)
        {
            BackingConsole = console ?? throw new ArgumentNullException(nameof(console));
        }

        /// <inheritdoc />
        public virtual ICapabilities Capabilities => BackingConsole.Capabilities;

        /// <inheritdoc />
        public virtual IConsoleCursor Cursor => BackingConsole.Cursor;

        /// <inheritdoc />
        public virtual IConsoleOutput Output => BackingConsole.Output;

        /// <inheritdoc />
        public virtual IConsoleInput Input => BackingConsole.Input;

        /// <inheritdoc />
        public virtual IConsoleWindow Window => BackingConsole.Window;

        /// <inheritdoc />
        public virtual IExclusivityMode ExclusivityMode { get; } = new DefaultExclusivityMode();

        /// <inheritdoc />
        public virtual RenderPipeline Pipeline { get; } = new();

        /// <summary>
        /// Gets the underlying console.
        /// </summary>
        protected IConsole BackingConsole { get; }

        /// <inheritdoc />
        public virtual void Clear(bool home)
        {
            Output.Clear(home);
        }

        /// <inheritdoc />
        public virtual void Write(IRenderable renderable)
        {
            this.GetSegments(renderable).ForEach(segment => Output.Write(segment));
            Output.Flush();
        }
    }

    /// <summary>
    /// Represents a console window without a backend.
    /// </summary>
    public class DummyWindow : IConsoleWindow
    {
        /// <inheritdoc />
        public string Title { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Width { get; set; }

        /// <inheritdoc />
        public int Height { get; set; }
    }

    /// <summary>
    /// Represents a console window backed by System.Console.
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
    /// Represents the ANSI output part of an <see cref="IConsole"/>.
    /// </summary>
    public class AnsiConsoleOutput : IConsoleOutput
    {
        private readonly AnsiBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiConsoleOutput"/> class.
        /// </summary>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="capabilities">The console capabilities.</param>
        public AnsiConsoleOutput(TextWriter standardOutput, ICapabilities capabilities)
        {
            _builder = new AnsiBuilder(capabilities);
            StandardOutput = standardOutput ?? throw new ArgumentNullException(nameof(standardOutput));
        }

        /// <summary>
        /// Gets the console output text writer.
        /// </summary>
        protected TextWriter StandardOutput { get; }

        /// <inheritdoc />
        public virtual void Clear(bool home)
        {
            Write(Segment.Control(ED(2)));
            Write(Segment.Control(ED(3)));

            if (home)
            {
                Write(Segment.Control(CUP(1, 1)));
            }

            Flush();
        }

        /// <inheritdoc />
        public virtual void Flush()
        {
            StandardOutput.Flush();
        }

        /// <inheritdoc />
        public virtual void Write(Segment segment)
        {
            if (segment.IsControlCode)
            {
                StandardOutput.Write(segment.Text);
                return;
            }

            var parts = segment.Text.NormalizeNewLines().Split(new[] { '\n' });
            foreach (var (_, _, last, part) in parts.Enumerate())
            {
                if (!string.IsNullOrEmpty(part))
                {
                    StandardOutput.Write(_builder.GetAnsi(part, segment.Style));
                }

                if (!last)
                {
                    StandardOutput.Write(Environment.NewLine);
                }
            }
        }
    }
}
