using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class BitbucketEnricher : ICapabilitiesEnricher
    {
        public string Name => "Bitbucket";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("BITBUCKET_REPO_OWNER") ||
                environmentVariables.ContainsKey("BITBUCKET_REPO_SLUG") ||
                environmentVariables.ContainsKey("BITBUCKET_COMMIT");
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
