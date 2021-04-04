namespace Spectre.Console.Testing
{
    public sealed class FakeAnsiConsoleCursor : IConsoleCursor
    {
        public void Move(CursorDirection direction, int steps)
        {
        }

        public void SetPosition(int column, int line)
        {
        }

        public void Show(bool show)
        {
        }
    }
}
