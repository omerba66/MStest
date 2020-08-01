using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MicrosoftAssignment.RateLimit
{
    public class Throttler
    {
        public int RequestLimit { get; }
        public int RequestsRemaining { get; private set; }
        public DateTime WindowResetDate { get; private set; }

        private static readonly ConcurrentDictionary<string, ThrottleInfo> Cache =
            new ConcurrentDictionary<string, ThrottleInfo>();

        public string ThrottleGroup { get; set; }
        private readonly int _timeoutInSeconds;

        public Throttler(string key, int requestLimit = 5, int timeoutInSeconds = 5)
        {
            RequestLimit = requestLimit;
            _timeoutInSeconds = timeoutInSeconds;
            ThrottleGroup = key;
        }

        private ThrottleInfo GetThrottleInfoFromCache()
        {
            Cache.TryGetValue(ThrottleGroup, out var throttleInfo);

            if (throttleInfo == null)
            {
                throttleInfo = CreateNewThrottleInfo();
            }
            else if(throttleInfo.ExpiresAt <= DateTime.Now)
            {
                Cache.TryRemove(ThrottleGroup, out throttleInfo);
                throttleInfo = CreateNewThrottleInfo();

            }

            return throttleInfo;
        }

        private ThrottleInfo CreateNewThrottleInfo()
        {
            var throttleInfo = new ThrottleInfo
            {
                ExpiresAt = DateTime.Now.AddSeconds(_timeoutInSeconds),
                RequestCount = 0
            };
            return throttleInfo;
        }

        public bool RequestShouldBeThrottled
        {
            get
            {
                var throttleInfo = GetThrottleInfoFromCache();
                WindowResetDate = throttleInfo.ExpiresAt;
                RequestsRemaining = Math.Max(RequestLimit - throttleInfo.RequestCount, 0);
                return (throttleInfo.RequestCount > RequestLimit);
            }
        }

        public void IncrementRequestCount()
        {
            Cache.AddOrUpdate(ThrottleGroup, new ThrottleInfo
            {
                ExpiresAt = DateTime.Now.AddSeconds(_timeoutInSeconds),
                RequestCount = 1
            }, (key, throttleInfo) =>
            {
                Interlocked.Increment(ref throttleInfo.RequestCount);
                return throttleInfo;
            });
        }
        public Dictionary<string, string> GetRateLimitHeaders()
        {
            var throttleInfo = GetThrottleInfoFromCache();

            var requestsRemaining = Math.Max(RequestLimit - throttleInfo.RequestCount, 0);

            var headers = new Dictionary<string, string>
            {
                {"RateLimit-Limit", RequestLimit.ToString()},
                {"RateLimit-Remaining", requestsRemaining.ToString()},
                {"RateLimit-Reset", ToUnixTime(throttleInfo.ExpiresAt.Date).ToString()}
            };
            return headers;
        }

        private long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }
        private class ThrottleInfo
        {
            public DateTime ExpiresAt;
            public int RequestCount;
        }

    }
}