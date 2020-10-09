# appconfig-examples

Provides examples on how to use Azure App Configurations

## Try it

Start by forking this repo :)

For trying it, you'll need an Azure subscription. You can get a free account [here](https://azure.microsoft.com/en-us/free/search/?&ef_id=EAIaIQobChMIhbL7upum7AIVGKSzCh34vA6BEAAYASAAEgK_nvD_BwE:G:s&OCID=AID2100014_SEM_EAIaIQobChMIhbL7upum7AIVGKSzCh34vA6BEAAYASAAEgK_nvD_BwE:G:s)

Most of the steps below use the Azure CLI, please refer to the [documentation](https://docs.microsoft.com/pt-br/cli/azure/) to get started, install and login.

### Creating the resources

For creating the resources, first create a resource group

```bash
az group create -n appconfig-examples -l brazilsouth
```

After the resource group is created, you can deploy the ARM template found at the **.arm** folder. If you don't know what an ARM template is, please check this [link](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/)

```bash
SITE_NAME=appconfig-examples-$RANDOM

az deployment group create -g appconfig-examples --template-file .arm/template.json --parameters appservice_name=$SITE_NAME
```

A few notes here...:

1. The variable **SITE_NAME** uses the **$RANDOM** function to concatenate a random number to the prefix **appconfig-examples-** since site names must be unique within azure. To see what is its value, run the following command:

```bash
echo $SITE_NAME
```

2. If you already have a free Azure App Configuration Store, the script is going to fail, since you're allowed to have only one free store per subscription

### Deploying

For deploying the app, this repo provides a Github Actions workflow. 

You'll need to do the following steps:

#### Download the publish profile

Go to the Azure Portal, find your web app and click "Get Publish Profile". It'll download the publish profile xml file. Copy the content and [create a Github Secret](https://docs.github.com/pt/free-pro-team@latest/actions/reference/encrypted-secrets#creating-encrypted-secrets-for-a-repository) named *AZURE_WEBAPP_PUBLISH_PROFILE* and paste the xml as the secret value.

#### Update site name

Open the file **.github/workflows/deploy-to-azure.yml** with ~vim~ your favorite text editor and set the variable **AZURE_WEBAPP_NAME** with your site name. Commit it to the main branch and push it to your fork. The workflow should deploy the site to azure!

### Testing

After the workflow is done sucessfully, run the following command into your terminal

```bash
 curl -X GET https://$SITE_NAME.azurewebsites.net/WeatherForecast
 ```

 You should see an output similar with

 ```json
[{"date":"2020-10-10T01:30:05.1297458+00:00","temperatureC":32,"temperatureF":89,"summary":"Cool"},{"date":"2020-10-11T01:30:05.1297617+00:00","temperatureC":-13,"temperatureF":9,"summary":"Mild"},{"date":"2020-10-12T01:30:05.1297719+00:00","temperatureC":39,"temperatureF":102,"summary":"Freezing"},{"date":"2020-10-13T01:30:05.1297819+00:00","temperatureC":-9,"temperatureF":16,"summary":"Mild"},{"date":"2020-10-14T01:30:05.1297919+00:00","temperatureC":-18,"temperatureF":0,"summary":"Freezing"},{"date":"2020-10-15T01:30:05.1298018+00:00","temperatureC":36,"temperatureF":96,"summary":"Mild"},{"date":"2020-10-16T01:30:05.1298118+00:00","temperatureC":0,"temperatureF":32,"summary":"Balmy"},{"date":"2020-10-17T01:30:05.1298219+00:00","temperatureC":41,"temperatureF":105,"summary":"Sweltering"},{"date":"2020-10-18T01:30:05.1298318+00:00","temperatureC":34,"temperatureF":93,"summary":"Scorching"},{"date":"2020-10-19T01:30:05.1298417+00:00","temperatureC":12,"temperatureF":53,"summary":"Sweltering"}]
 ```