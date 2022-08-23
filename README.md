# Tweet Counter
Twitter has become a popular place for customers to interact with companies and vice-versa. This could be taken as a sign of customer service as customers use Twitter to raise questions.

The aim of Tweet Counter is to visualise daily tweet counts so that the public can see to what level companies are interacting.

Is a high volumn of tweets better customer service than a low volume? Or is the other way around better? That is open to interpretation, but Tweet Counter will hopefully hep with that decision.

## Deployment
### Secrets
There are seveal secrets that Tweet Counter relies on. These are stored in a Key Vault, but some of the steps below will require them as well. Therefore it is necessary to setup the following repository secrets in your repo:

| Name                     | Description                                      |
| ------------------------ | ------------------------------------------------ |
| TWITTER_API_BEARER_TOKEN | The bearer token of your Twitter API Application |

### Infrastructure
The infrastructure is written as Infrastructure as Code, using Microsofts Bicep language.

This repo deploys the main.bicep file through a GitHub Action, but you can also run the file directly with the same outcome.

The infrastructure in this repo is deployed via GitHub Actions. While you could also do the same, it is also possible to just run 
The resources for this application are created using [Bicep](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep)

### GitHub Action
To be able to deploy the Bicep file via a GitHub Action, you will need to carry out a couple of steps. Microsoft has documentation on this, so the links are:
1. [Generate deployment credentials](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/deploy-github-actions?tabs=userlevel%2CCLI#generate-deployment-credentials)
1. [Configure the GitHub secrets](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/deploy-github-actions?tabs=userlevel%2CCLI#configure-the-github-secrets)

Once you have completed those steps, you can use the Action in this repo as an example.

### Deploy the Bicep file
Deploy the Bicep file using Azure CLI with the following commands. Substituting the resource group name and location to your needs:

```bash
az group create --name exampleRG --location eastus
az deployment group create --resource-group tweetcounter --template-file main.bicep --parameters twitterApiBearerToken=AAAAAAAAAAAAAAAAAAAAAPRwgAEAAAAAKZDosHMf0PRpPtLXpjxqFJVkw2I%3DOvC55lX2xx6gsofMPeonqzH38AJOWB9O0AS5oDWAqw3lgtXiuw
```

Where secrets are involved, such as the Twitter API bearer token, these parameters are defined as secrets in the Bicep file. They will therefore not be saved in the deployment.