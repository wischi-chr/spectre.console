using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Testing
{
    public sealed class FakeConsole : IAnsiConsole, IDisposable
    {
        public IProfile Profile { get; }
        public IAnsiConsoleCursor Cursor => new FakeAnsiConsoleCursor();
        IAnsiConsoleInput IAnsiConsole.Input => Input;
        public IExclusivityMode ExclusivityMode { get; }
        public RenderPipeline Pipeline { get; }

        public FakeConsoleInput Input { get; }
        public string Output => Profile.StandardOutput.ToString();
        public IReadOnlyList<string> Lines => Output.TrimEnd('\n').Split(new char[] { '\n' });

        public FakeConsole(
            int width = 80, int height = 9000,
            bool supportsAnsi = true, ColorSystem colorSystem = ColorSystem.Standard,
            bool legacyConsole = false, bool interactive = true)
        {
            Input = new FakeConsoleInput();
            ExclusivityMode = new FakeExclusivityMode();
            Pipeline = new RenderPipeline();

            Profile = new Profile(new StringWriter())
            {
                Width = width,
                Height = height,
                Capabilities = new Capabilities()
                {
                    ColorSystem = colorSystem,
                    Ansi = supportsAnsi,
                    Legacy = legacyConsole,
                    Interactive = interactive,
                    Links = true,
                    Unicode = true,
                },
            };
        }

        public void Dispose()
        {
            Profile.StandardOutput.Dispose();
        }

        public void Clear(bool home)
        {
        }

        public void Write(IRenderable renderable)
        {
            foreach (var segment in renderable.GetSegments(this))
            {
                Profile.StandardOutput.Write(segment.Text);
            }
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
}
