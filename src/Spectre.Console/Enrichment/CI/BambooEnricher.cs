using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class BambooEnricher : ICapabilitiesEnricher
    {
        public string Name => "Bamboo";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("bamboo_buildNumber");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
