using System;
using System.Linq;
using static Spectre.Console.AnsiSequences;

namespace Spectre.Console
{
    internal sealed class AnsiBuilder
    {
        private readonly ICapabilities _capabilities;
        private readonly AnsiLinkHasher _linkHasher;

        public AnsiBuilder(ICapabilities capabilities)
        {
            _capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            _linkHasher = new AnsiLinkHasher();
        }

        public string GetAnsi(string text, Style style)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            var codes = AnsiDecorationBuilder.GetAnsiCodes(style.Decoration);

            // Got foreground?
            if (style.Foreground != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        _capabilities.ColorSystem,
                        style.Foreground,
                        true));
            }

            // Got background?
            if (style.Background != Color.Default)
            {
                codes = codes.Concat(
                    AnsiColorBuilder.GetAnsiCodes(
                        _capabilities.ColorSystem,
                        style.Background,
                        false));
            }

            var result = codes.ToArray();
            if (result.Length == 0 && style.Link == null)
            {
                return text;
            }

            var ansi = result.Length > 0
                ? $"{SGR(result)}{text}{SGR(0)}"
                : text;

            if (style.Link != null && !_capabilities.Legacy)
            {
                var link = style.Link;

                // Empty links means we should take the URL from the text.
                if (link.Equals(Constants.EmptyLink, StringComparison.Ordinal))
                {
                    link = text;
                }

                var linkId = _linkHasher.GenerateId(link, text);
                ansi = $"{CSI}]8;id={linkId};{link}{CSI}\\{ansi}{CSI}]8;;{CSI}\\";
            }

            return ansi;
        }
    }
}
