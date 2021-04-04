using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IRenderable"/>.
    /// </summary>
    public static class RenderableExtensions
    {
        /// <summary>
        /// Gets the segments for a renderable using the specified console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="renderable">The renderable.</param>
        /// <returns>An enumerable containing segments representing the specified <see cref="IRenderable"/>.</returns>
        public static IEnumerable<Segment> GetSegments(this IAnsiConsole console, IRenderable renderable)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            var context = new RenderContext(console.Capabilities);
            var renderables = console.Pipeline.Process(context, new[] { renderable });

            return GetSegments(console, context, renderables);
        }

        private static IEnumerable<Segment> GetSegments(IAnsiConsole console, RenderContext options, IEnumerable<IRenderable> renderables)
        {
            var result = new List<Segment>();
            foreach (var renderable in renderables)
            {
                result.AddRange(renderable.Render(options, console.Output.Width));
            }

            return Segment.Merge(result);
        }
    }
}