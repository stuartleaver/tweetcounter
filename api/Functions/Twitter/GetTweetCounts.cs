using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TweetCounter.Api.Services.Interfaces;

namespace TweetCounter.Api.Functions.Twitter;

public class GetTweetCounts
{
    private readonly ITwitterService _twitterService;

    private readonly ILogger<GetTweetCounts> _logger;

    public GetTweetCounts(ITwitterService twitterService, ILogger<GetTweetCounts> logger)
    {
        _twitterService = twitterService;

        _logger = logger;
    }
    
    [FunctionName("GetTweetCounts")]
    public async Task RunAsync(
        [TimerTrigger("0 0 2 * * *")] TimerInfo myTimer,
        [Table("DailyTweetCounts")] TableClient table,
        [Table("TwitterUser")] TableClient usersTable,
        ILogger log)
    {
        log.LogInformation($"AddTwitterAccount called at {DateTime.UtcNow}");
        
        try
        {
            var userIds = GetUsers("UkRailSatisfaction", usersTable);

            foreach (var id in userIds)
            {
                var tweetCount = await _twitterService.GetTweetCountByUserId(id);
        
                if (tweetCount == null)
                {
                    _logger.LogError($"Tweet counts for user id {id} not found");
                }

                await table.UpsertEntityAsync(tweetCount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured in GetTweetCountByUserId, Exception: {ex.Message}");
        }
    }

    private List<long> GetUsers(string partitionKey, TableClient usersTable)
    {
        var users = new List<long>();
        
        var queryResultsFilter = usersTable.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");
        
        foreach (var entity in queryResultsFilter)
        {
            var userId = entity.GetInt64("Id");
        
            if (userId != null)
            {
                users.Add((long)userId);
            }
        }

        return users;
    }
}