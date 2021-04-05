using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Testing
{
    public sealed class FakeConsole : ConsoleFacade, IDisposable
    {
        private readonly FakeConsoleBackend _fakeConsole;

        public FakeConsole(FakeConsoleBackend fakeConsole)
            : base(fakeConsole)
        {
            _fakeConsole = fakeConsole ?? throw new ArgumentNullException(nameof(fakeConsole));
        }

        public override IExclusivityMode ExclusivityMode { get; } = new FakeExclusivityMode();

        public new string Output => _fakeConsole.StandardOutput.ToString();
        public new FakeConsoleInput Input => _fakeConsole.Input;
        public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(new char[] { '\n' });

        public void Dispose()
        {
            _fakeConsole.Dispose();
        }

        public static FakeConsole Create(
            int width = 80,
            int height = 9000,
            bool supportsAnsi = true,
            bool legacyConsole = false,
            bool interactive = true,
            ColorSystem colorSystem = ColorSystem.Standard,
            bool suppressAnsiOutput = true)
        {
            var backend = new FakeConsoleBackend(suppressAnsiOutput);

            backend.Window.Width = width;
            backend.Window.Height = height;

            backend.Capabilities.Ansi = supportsAnsi;
            backend.Capabilities.ColorSystem = colorSystem;
            backend.Capabilities.Legacy = legacyConsole;
            backend.Capabilities.Interactive = interactive;

            return new FakeConsole(backend);
        }

        public static FakeConsole Create(
            ColorSystem system,
            AnsiSupport ansi = AnsiSupport.Yes,
            int width = 80)
        {
            return Create(
                colorSystem: system,
                supportsAnsi: ansi == AnsiSupport.Yes,
                width: width,
                suppressAnsiOutput: false
            );
        }

        public string WriteNormalizedException(Exception ex, ExceptionFormats formats = ExceptionFormats.Default)
        {
            this.WriteException(ex, formats);

            return string.Join("\n", Output.NormalizeStackTrace()
                .NormalizeLineEndings()
                .Split(new char[] { '\n' })
                .Select(line => line.TrimEnd()));
        }
    }

    public sealed class FakeAnsiConsoleOutput : AnsiConsoleOutput
    {
        private readonly bool _suppressAnsiOutput;

        public FakeAnsiConsoleOutput(
            TextWriter standardOutput,
            ICapabilities capabilities,
            bool suppressAnsiOutput)
            : base(standardOutput, capabilities)
        {
            _suppressAnsiOutput = suppressAnsiOutput;
        }

        public override void Write(Segment segment)
        {
            if (_suppressAnsiOutput)
            {
                StandardOutput.Write(segment.Text);
            }
            else
            {
                base.Write(segment);
            }
        }
    }

    public sealed class FakeConsoleBackend : IConsole, IDisposable
    {
        public FakeConsoleBackend(bool suppressAnsiOutput)
        {
            Output = new FakeAnsiConsoleOutput(StandardOutput, Capabilities, suppressAnsiOutput);
        }

        public DummyWindow Window { get; } = new();
        public FakeConsoleInput Input { get; } = new();
        public Capabilities Capabilities { get; } = new();
        public FakeAnsiConsoleCursor Cursor { get; } = new();
        public StringWriter StandardOutput { get; } = new();
        public AnsiConsoleOutput Output { get; }

        ICapabilities IConsole.Capabilities => Capabilities;
        IConsoleCursor IConsole.Cursor => Cursor;
        IConsoleInput IConsole.Input => Input;
        IConsoleWindow IConsole.Window => Window;
        IConsoleOutput IConsole.Output => Output;

        public void Dispose()
        {
            StandardOutput.Dispose();
        }
    }
}
