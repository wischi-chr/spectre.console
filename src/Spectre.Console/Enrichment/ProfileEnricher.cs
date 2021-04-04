using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Enrichment
{
    internal static class CapabilitiesEnricher

    {
        private static readonly List<ICapabilitiesEnricher> _defaultEnrichers = new()
        {
            new AppVeyorEnricher(),
            new BambooEnricher(),
            new BitbucketEnricher(),
            new BitriseEnricher(),
            new ContinuaEnricher(),
            new GitHubEnricher(),
            new GitLabEnricher(),
            new GoCDEnricher(),
            new JenkinsEnricher(),
            new MyGetEnricher(),
            new TeamCityEnricher(),
            new TfsEnricher(),
            new TravisEnricher(),
        };

        public static void Enrich(
            Capabilities capabilities,
            CapabilitiesEnrichment settings,
            IDictionary<string, string>? environmentVariables)
        {
            if (capabilities is null)
            {
                throw new ArgumentNullException(nameof(capabilities));
            }

            settings ??= new CapabilitiesEnrichment();

            var variables = GetEnvironmentVariables(environmentVariables);
            foreach (var enricher in GetEnrichers(settings))
            {
                if (string.IsNullOrWhiteSpace(enricher.Name))
                {
                    throw new InvalidOperationException($"Profile enricher of type '{enricher.GetType().FullName}' does not have a name.");
                }

                if (enricher.Enabled(variables))
                {
                    enricher.Enrich(capabilities);

                    // TODO: Fix that ?
                    // profile.AddEnricher(enricher.Name);
                }
            }
        }

        private static List<ICapabilitiesEnricher> GetEnrichers(CapabilitiesEnrichment settings)
        {
            var enrichers = new List<ICapabilitiesEnricher>();

            if (settings.UseDefaultEnrichers)
            {
                enrichers.AddRange(_defaultEnrichers);
            }

            if (settings.Enrichers?.Count > 0)
            {
                enrichers.AddRange(settings.Enrichers);
            }

            return enrichers;
        }

        private static IDictionary<string, string> GetEnvironmentVariables(IDictionary<string, string>? variables)
        {
            if (variables != null)
            {
                return new Dictionary<string, string>(variables, StringComparer.OrdinalIgnoreCase);
            }

            return Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>()
                .Aggregate(
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                    (dictionary, entry) =>
                    {
                        var key = (string)entry.Key;
                        if (!dictionary.TryGetValue(key, out _))
                        {
                            dictionary.Add(key, entry.Value as string ?? string.Empty);
                        }

                        return dictionary;
                    },
                    dictionary => dictionary);
        }
    }
}
