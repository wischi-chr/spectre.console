using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class AppVeyorEnricher : ICapabilitiesEnricher
    {
        public string Name => "AppVeyor";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("APPVEYOR");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
