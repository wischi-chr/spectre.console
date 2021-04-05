using System;
using System.Collections.Generic;

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
            IDictionary<string, string> variables)
        {
            if (capabilities is null)
            {
                throw new ArgumentNullException(nameof(capabilities));
            }

            settings ??= new CapabilitiesEnrichment();

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
    }
}
