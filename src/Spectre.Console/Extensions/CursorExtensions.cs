namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IConsoleCursor"/>.
    /// </summary>
    public static class CursorExtensions
    {
        /// <summary>
        /// Shows the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void Show(this IConsoleCursor cursor)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Show(true);
        }

        /// <summary>
        /// Hides the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void Hide(this IConsoleCursor cursor)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Show(false);
        }

        /// <summary>
        /// Moves the cursor up.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void MoveUp(this IConsoleCursor cursor)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Up, 1);
        }

        /// <summary>
        /// Moves the cursor up.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveUp(this IConsoleCursor cursor, int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Up, steps);
        }

        /// <summary>
        /// Moves the cursor down.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void MoveDown(this IConsoleCursor cursor)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Down, 1);
        }

        /// <summary>
        /// Moves the cursor down.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveDown(this IConsoleCursor cursor, int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Down, steps);
        }

        /// <summary>
        /// Moves the cursor to the left.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void MoveLeft(this IConsoleCursor cursor)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Left, 1);
        }

        /// <summary>
        /// Moves the cursor to the left.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveLeft(this IConsoleCursor cursor, int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Left, steps);
        }

        /// <summary>
        /// Moves the cursor to the right.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public static void MoveRight(this IConsoleCursor cursor)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Right, 1);
        }

        /// <summary>
        /// Moves the cursor to the right.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="steps">The number of steps to move the cursor.</param>
        public static void MoveRight(this IConsoleCursor cursor, int steps)
        {
            if (cursor is null)
            {
                throw new System.ArgumentNullException(nameof(cursor));
            }

            cursor.Move(CursorDirection.Right, steps);
        }
    }
}
