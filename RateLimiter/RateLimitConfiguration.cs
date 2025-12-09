namespace MovieStream.Api.RateLimiter
{
    using System.Collections.Generic;
    using System.Linq;

    public class RateLimitConfiguration
    {
        private readonly List<(string Pattern, int Limit, int Window)> _wildcardLimits;

        public RateLimitConfiguration()
        {
            var wildcardRules = new List<(string Pattern, int Limit, int Window)>
        {
            ("/api/user/*", 3, 30),
            ("/*", 5, 30)
        };
            _wildcardLimits = wildcardRules
                .OrderByDescending(r => r.Pattern.Length)
                .ToList();
        }

        public bool TryGetLimit(string path, out int limit, out int windowInSeconds)
        {
            foreach (var entry in _wildcardLimits)
            {
                var pattern = entry.Pattern;

                if (pattern.EndsWith("/*"))
                {
                    var prefix = pattern.Substring(0, pattern.Length - 1);

                    if (path != null && path.StartsWith(prefix))
                    {
                        limit = entry.Limit;
                        windowInSeconds = entry.Window;
                        return true;
                    }
                }
            }

            limit = 0;
            windowInSeconds = 0;
            return false;
        }
    }
}
