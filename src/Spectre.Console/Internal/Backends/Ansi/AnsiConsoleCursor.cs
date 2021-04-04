using System;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleCursor : IConsoleCursor
    {
        private readonly AnsiConsoleBackend _backend;

        public AnsiConsoleCursor(AnsiConsoleBackend backend)
        {
            _backend = backend ?? throw new ArgumentNullException(nameof(backend));
        }

        public void Show(bool show)
        {
            if (show)
            {
                _backend.Write(new ControlSequence(SM(DECTCEM)));
            }
            else
            {
                _backend.Write(new ControlSequence(RM(DECTCEM)));
            }
        }

        public void Move(CursorDirection direction, int steps)
        {
            if (steps == 0)
            {
                return;
            }

            switch (direction)
            {
                case CursorDirection.Up:
                    _backend.Write(new ControlSequence(CUU(steps)));
                    break;
                case CursorDirection.Down:
                    _backend.Write(new ControlSequence(CUD(steps)));
                    break;
                case CursorDirection.Right:
                    _backend.Write(new ControlSequence(CUF(steps)));
                    break;
                case CursorDirection.Left:
                    _backend.Write(new ControlSequence(CUB(steps)));
                    break;
            }
        }

        public void SetPosition(int column, int line)
        {
            _backend.Write(new ControlSequence(CUP(line, column)));
        }
    }
}
