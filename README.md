# AzureBlobStorage

#### Step 1. Setting Up Azure Blob Storage
Before we proceed, you need to create an Azure Storage Account. If you’re unsure how to create an Azure Storage Account, refer to the article “Understanding and Creation of Azure Storage Account”. Once your storage account is created, navigate to your storage account and go to the “Access Key” section to get your connection string. Copy your connection string from here and keep it for the next step.

![image](https://github.com/user-attachments/assets/43b23104-b433-420d-a632-a679f6dbe4de)

#### Step 2. Create Project
ASP.NET Core Web API

#### Step 3: Install Required NuGet Package
```
dotnet add package Azure.Storage.Blobs
```
#### Step 4. Configuring the ASP.NET Core Application
In your ASP.NET Core project, open the appsettings.json file. This file contains configuration settings for your application. Add the following configuration snippet, replacing “YourConnectionStringHere” with the connection string obtained from the Azure portal:

```
{
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=your_account_name;AccountKey=your_account_key;EndpointSuffix=core.windows.net",
    "ContainerName": "your-container-name"
  }
}
```
