using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TweetCounter.Api.Extensions;
using TweetCounter.Api.Models.Twitter;
using TweetCounter.Api.Services.Interfaces;

namespace TweetCounter.Api.Services;

public class TwitterService : ITwitterService
{
    private readonly HttpClient _client;

    private readonly ILogger<TwitterService> _logger;

    public TwitterService(IHttpClientFactory httpClientFactory, ILogger<TwitterService> logger)
    {
        _client = httpClientFactory.CreateClient(nameof(TwitterService));

        _logger = logger;
    }
    
    public async Task<TwitterUser> GetUserByUsername(string username)
    {
        try
        {
            var response = await _client.GetAsync($"/2/users/by/username/{username}?user.fields=profile_image_url,created_at");
            
            var json = await response.Content.ReadAsStringAsync();

            var parsedUser = JsonNode.Parse(json)?["data"]?.ToString();

            var user = JsonConvert.DeserializeObject<TwitterUser>(parsedUser);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured in GetUserByUsername with parameters - Username: {username}, Exception: {ex.Message}");
            
            throw;
        }
    }

    public async Task<TwitterTweetCount> GetTweetCountByUserId(long userId)
    {
        var yesterday = DateTime.Today.AddDays(-1);

        var startTime = yesterday.StartOfDay().ToString("yyyy-MM-ddTHH:mm:ss.fff");
        var endTime = yesterday.EndOfDay().ToString("yyyy-MM-ddTHH:mm:ss.fff");
        
        try
        {
            var response = await _client.GetAsync($"/2/tweets/counts/recent?query=from:{userId}&start_time={startTime}z&end_time={endTime}z&granularity=day");
            
            var json = await response.Content.ReadAsStringAsync();

            var parsedTweetCount = JsonNode.Parse(json)?["data"]?[0]?.ToString();

            var tweetCount = JsonConvert.DeserializeObject<TwitterTweetCount>(parsedTweetCount);

            tweetCount.UserId = userId;
            
            tweetCount.RowKey = $"{userId}-{tweetCount.Start.Date.ToString("yyyyMMdd")}";

            return tweetCount;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured in GetUserByUsername with parameters - Username: {userId}, Exception: {ex.Message}");
            
            throw;
        }
    }
}