using System;
using Spectre.Console.Rendering;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a console cursor that uses ANSI commands to control the cursor.
    /// </summary>
    public sealed class AnsiConsoleCursor : IConsoleCursor
    {
        private readonly IConsoleOutput _ansiOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiConsoleCursor"/> class.
        /// </summary>
        /// <param name="ansiOutput">The output the ANSI cursor commands are written to.</param>
        public AnsiConsoleCursor(IConsoleOutput ansiOutput)
        {
            _ansiOutput = ansiOutput ?? throw new ArgumentNullException(nameof(ansiOutput));
        }

        /// <inheritdoc />
        public void Show(bool show)
        {
            var control = show ? SM(DECTCEM) : RM(DECTCEM);
            WriteControl(control);
        }

        /// <inheritdoc />
        public void Move(CursorDirection direction, int steps)
        {
            if (steps == 0)
            {
                return;
            }

            var control = direction switch
            {
                CursorDirection.Up => CUU(steps),
                CursorDirection.Down => CUD(steps),
                CursorDirection.Left => CUB(steps),
                CursorDirection.Right => CUF(steps),
                _ => throw new NotSupportedException($"Direction '{direction}' not supported."),
            };

            WriteControl(control);
        }

        /// <inheritdoc />
        public void SetPosition(int column, int line)
        {
            WriteControl(CUP(line, column));
        }

        private void WriteControl(string control)
        {
            _ansiOutput.Write(Segment.Control(control));
        }
    }
}
