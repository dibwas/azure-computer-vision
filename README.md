# azure-computer-vision
Leveraging Azure Cognitive Service - Computer Vision for batch processing Optical Character Recognition

After creating a Cognitive Service resource in the Azure portal, you'll get an endpoint and a key for authenticating your applications.

From the quickstart pane that opens, you can access your key and endpoint.

Configure an environment variable for authentication

To set the environment variable, open a console window, and follow the instructions for your operating system. Replace your-key with one of the keys for your resource.

```powershell
setx COGNITIVE_SERVICE_KEY "your-key"
```
The preferred way of storing such secrets for development is creating local.settings.json. Once your application is deployed these keys need to be either red from Azure Keyvaults or Azure DevOps variable groups, for both cases make sure you turned on system assigned identity.

Run SetEnvVars.cs with argument pointing at local.settings.json
Such file is structured as follows:

```json
{
  "IsEncrypted": false,
  "Values": {
    "COGNITIVE_SERVICE_NAME": "kinda secretish",
    "COGNITIVE_SERVICE_ENDPOINT": "secret",
	"COGNITIVE_SERVICE_KEY": "Woooiii very secret"
  }
}
```

Restart your IDE



### Authenticate requests to Azure Cognitive Services

https://docs.microsoft.com/en-us/azure/cognitive-services/authentication


Plus with postman



### Common development errors
While importing 
```C#
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
```
Or
```C#
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
```
You might get
```
Error	CS0234	The type or namespace name 'Azure' does not exist in the namespace 'Microsoft' (are you missing an assembly reference?)
```

#### Fix
Make sure you have latest version of [.Net Core SDKs](https://dotnet.microsoft.com/download/visual-studio-sdks) both visual studio and runtime

Make sure you have latest version of [Computer Vision NuGet package](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Vision.ComputerVision/) installed

Run the following command in PowerShell to get it \o/
```Powershell
dotnet add package Microsoft.Azure.CognitiveServices.Vision.ComputerVision --version 5.0.0
```


### [Testing](OCRVisionApp/InputImages/README.md)