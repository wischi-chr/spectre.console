using System;
using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class GitLabEnricher : ICapabilitiesEnricher
    {
        public string Name => "GitLab";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            if (environmentVariables.TryGetValue("CI_SERVER", out var value))
            {
                return value?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? false;
            }

            return false;
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
