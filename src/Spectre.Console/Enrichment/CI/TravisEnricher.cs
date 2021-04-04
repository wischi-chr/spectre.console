using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class TravisEnricher : ICapabilitiesEnricher
    {
        public string Name => "Travis";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TRAVIS");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
