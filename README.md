# document-management-api

## Welcome to the document-management-api wiki!

I am storing my data at **Azure Blob** and **Azure Cosmos Table**.

In order to have a possibility to test application workflow locally, please use [**Azure Storage Emulator**](https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/storage/common/storage-use-emulator.md). 
Also, [*Azure Storage Explorer*](https://azure.microsoft.com/en-us/features/storage-explorer/) is used as a GUI tool for working with Azure storage emulator.

Before running app locally, also please add the following connection string to _appsettings.json_

```json
	{
		"StorageConnectionString": "UseDevelopmentStorage=true"
	}
```