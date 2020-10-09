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

Run the following command to get your publish profile

```bash
 az webapp deployment list-publishing-profiles -g appconfig-examples -n $SITE_NAME --xml
```

Copy the output and [create a Github Secret](https://docs.github.com/pt/free-pro-team@latest/actions/reference/encrypted-secrets#creating-encrypted-secrets-for-a-repository) named *AZURE_WEBAPP_PUBLISH_PROFILE* and paste the xml as the secret value

#### Update site name

Open the file **.github/workflows/deploy-to-azure.yml** with ~vim~ your favorite text editor and set the variable **AZURE_WEBAPP_NAME** with your site name. Commit it to the main branch and push it to your fork. The workflow should deploy the site to azure!