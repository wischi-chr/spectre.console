using System;
using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class MyGetEnricher : ICapabilitiesEnricher
    {
        public string Name => "MyGet";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            if (environmentVariables.TryGetValue("BuildRunner", out var value))
            {
                return value?.Equals("MyGet", StringComparison.OrdinalIgnoreCase) ?? false;
            }

            return false;
        }

        public void Enrich(Capabilities capabilities)
        {
            capabilities.Interactive = false;
        }
    }
}
