using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleFacade : IAnsiConsole
    {
        private readonly object _renderLock = new();
        private readonly AnsiConsoleBackend _ansiBackend;
        private readonly LegacyConsoleBackend _legacyBackend;

        public IProfile Profile { get; }
        public IAnsiConsoleCursor Cursor => GetBackend().Cursor;
        public IConsoleInput Input { get; }
        public IExclusivityMode ExclusivityMode { get; }
        public RenderPipeline Pipeline { get; }

        public AnsiConsoleFacade(IProfile profile, IExclusivityMode exclusivityMode)
        {
            Profile = profile ?? throw new ArgumentNullException(nameof(profile));
            Input = new DefaultInput(Profile);
            ExclusivityMode = exclusivityMode ?? throw new ArgumentNullException(nameof(exclusivityMode));
            Pipeline = new RenderPipeline();

            _ansiBackend = new AnsiConsoleBackend(this);
            _legacyBackend = new LegacyConsoleBackend(this);
        }

        public void Clear(bool home)
        {
            lock (_renderLock)
            {
                GetBackend().Clear(home);
            }
        }

        public void Write(IRenderable renderable)
        {
            lock (_renderLock)
            {
                GetBackend().Write(renderable);
            }
        }

        private IAnsiConsoleBackend GetBackend()
        {
            if (Profile.Capabilities.Ansi)
            {
                return _ansiBackend;
            }

            return _legacyBackend;
        }
    }
}
