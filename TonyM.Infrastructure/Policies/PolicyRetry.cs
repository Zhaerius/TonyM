using Polly;
using Polly.Extensions.Http;

namespace TonyM.Infrastructure.Policies
{
    public static class PolicyRetry
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(1000));
        }
    }
}
