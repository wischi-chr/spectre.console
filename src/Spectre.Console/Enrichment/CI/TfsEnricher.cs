using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class TfsEnricher : ICapabilitiesEnricher
    {
        public string Name => "TFS";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TF_BUILD");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
