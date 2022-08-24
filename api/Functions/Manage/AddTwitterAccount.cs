using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TweetCounter.Api.Services.Interfaces;

namespace TweetCounter.Api.Functions.Manage;

public class AddTwitterAccount
{
    private readonly ITwitterService _twitterService;

    private readonly ILogger<AddTwitterAccount> _logger;

    public AddTwitterAccount(ITwitterService twitterService, ILogger<AddTwitterAccount> logger)
    {
        _twitterService = twitterService;

        _logger = logger;
    }
    
    [FunctionName("AddTwitterAccount")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "manage/addtwitteraccount")] HttpRequest req,
        [Table("TwitterUser")] TableClient table,
        ILogger log)
    {
        log.LogInformation($"AddTwitterAccount called at {DateTime.UtcNow}");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string username = data?.username;
        
        if (string.IsNullOrEmpty(username))
        {
            return new BadRequestObjectResult("Please pass a username in the request body");
        }

        try
        {
            var user = await _twitterService.GetUserByUsername(username);
        
            if (user == null)
            {
                return new NotFoundObjectResult($"Username {username} not found");
            }

            await table.UpsertEntityAsync(user);
        
            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured in GetUserByUsername with parameters - Username: {username}, Exception: {ex.Message}");
        
            return new InternalServerErrorResult();
        }
    }
}