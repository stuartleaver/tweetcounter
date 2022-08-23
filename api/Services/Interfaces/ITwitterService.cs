using System.Threading.Tasks;
using TweetCounter.Api.Models.Twitter;

namespace TweetCounter.Api.Services.Interfaces;

public interface ITwitterService
{
    Task<TwitterUser> GetUserByUsername(string username);

    Task<TwitterTweetCount> GetTweetCountByUserId(long userId);
}