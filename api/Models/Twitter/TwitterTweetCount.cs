using System;
using Newtonsoft.Json;
using TweetCounter.Api.Enums;
using TweetCounter.Api.Extensions;

namespace TweetCounter.Api.Models.Twitter;

public class TwitterTweetCount : BaseTableEntity
{
    [JsonProperty("start")]
    public DateTime Start { get; set; }
    
    [JsonProperty("end")]
    public DateTime End { get; set; }
    
    [JsonProperty("tweet_count")]
    public int TweetCount { get; set; }
    
    public long UserId { get; set; }

    [JsonConstructor]
    public TwitterTweetCount(DateTime start, DateTime end, int tweetCount)
    {
        PartitionKey = TwitterUserPartitionKey.UkRailSatisfaction.GetEnumDescription();
        
        Start = start;
        
        End = end;
        
        TweetCount = tweetCount;
    }
}