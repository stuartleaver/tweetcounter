using System;
using System.Net.Http.Headers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TweetCounter.Api.Services;
using TweetCounter.Api.Services.Interfaces;

[assembly: FunctionsStartup(typeof(TweetCounter.Api.Startup))]

namespace TweetCounter.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<TwitterService>(client =>
            {
                client.BaseAddress = new Uri("https://api.twitter.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    Environment.GetEnvironmentVariable("TwitterApiBearerToken"));
            });

            builder.Services.AddSingleton<ITwitterService, TwitterService>();

            builder.Services.AddLogging();
        }
    }
}