using System;
using Newtonsoft.Json;
using TweetCounter.Api.Enums;
using TweetCounter.Api.Extensions;

namespace TweetCounter.Api.Models.Twitter;

public class TwitterUser : BaseTableEntity
{
    [JsonProperty("id")]
    public ulong Id { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("profile_image_url")]
    public string ProfileImageUrl { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonConstructor]
    public TwitterUser(ulong id, string username, string name, string profileImageUrl, DateTime createdAt)
    {
        PartitionKey = TwitterUserPartitionKey.UkRailSatisfaction.GetEnumDescription();
        
        RowKey = id.ToString();
        
        Id = id;
        
        Username = username;
        
        Name = name;
        
        ProfileImageUrl = profileImageUrl;
        
        CreatedAt = createdAt;
    }
}