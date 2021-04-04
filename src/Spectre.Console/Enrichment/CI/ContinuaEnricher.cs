using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class ContinuaEnricher : ICapabilitiesEnricher
    {
        public string Name => "ContinuaCI";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("ContinuaCI.Version");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
