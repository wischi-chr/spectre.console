using System;
using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class GitHubEnricher : ICapabilitiesEnricher
    {
        public string Name => "GitHub";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            if (environmentVariables.TryGetValue("GITHUB_ACTIONS", out var value))
            {
                return value?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
            }

            return false;
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Ansi = true;
            capabilities.Legacy = false;
            capabilities.Interactive = false;
            capabilities.Links = false;
        }
    }
}
