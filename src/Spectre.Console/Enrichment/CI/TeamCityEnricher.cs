using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class TeamCityEnricher : ICapabilitiesEnricher
    {
        public string Name => "TeamCity";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TEAMCITY_VERSION");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
