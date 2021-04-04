using System;

namespace Spectre.Console
{
    internal sealed class DefaultInput : IConsoleInput
    {
        private readonly IProfile _profile;

        public DefaultInput(IProfile profile)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (!_profile.Capabilities.Interactive)
            {
                throw new InvalidOperationException("Failed to read input in non-interactive mode.");
            }

            return _profile.ReadKey(intercept);
        }
    }
}
