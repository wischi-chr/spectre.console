using Spectre.Console;

namespace InfoExample
{
    public static class Program
    {
        public static void Main()
        {
            var grid = new Grid()
                .AddColumn(new GridColumn().NoWrap().PadRight(4))
                .AddColumn()
                .AddRow("[b]Color system[/]", $"{AnsiConsole.Console.Capabilities.ColorSystem}")
                .AddRow("[b]Unicode?[/]", $"{YesNo(AnsiConsole.Console.Capabilities.Unicode)}")
                .AddRow("[b]Supports ansi?[/]", $"{YesNo(AnsiConsole.Console.Capabilities.Ansi)}")
                .AddRow("[b]Supports links?[/]", $"{YesNo(AnsiConsole.Console.Capabilities.Links)}")
                .AddRow("[b]Legacy console?[/]", $"{YesNo(AnsiConsole.Console.Capabilities.Legacy)}")
                .AddRow("[b]Interactive?[/]", $"{YesNo(AnsiConsole.Console.Capabilities.Interactive)}")
                .AddRow("[b]Buffer width[/]", $"{AnsiConsole.Console.Output.Width}")
                .AddRow("[b]Buffer height[/]", $"{AnsiConsole.Console.Output.Height}");

            AnsiConsole.Render(
                new Panel(grid)
                    .Header("Information"));
        }

        private static string YesNo(bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
