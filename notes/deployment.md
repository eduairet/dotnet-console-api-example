# Deployment

-   We need an Azure account
-   We'll use the command line to deploy our app
    -   First login to Azure
        ```SHELL
        az login
        ```
    -   Then create the release version of our app
        ```SHELL
        dotnet build --configuration Release
        ```
        -   In the output we'll see the path of the release version of our app
            ```SHELL
            C:\User-Path\DotnetAPI\bin\Release\net8.0\DotnetAPI.dll
            ```
    -   Then we're going to deploy it in the cloud using the free tier
        ```SHELL
        az webapp up --sku F1 -name <name-of-your-app> --os-type <linux | windows | etc>
        ```
        -   The output will be the information of your app, for example
            ```JSON
            {
                "URL": "http://dotnetapiname.azurewebsites.net",
                "appserviceplan": "appserviceplan-name",
                "location": "location-name",
                "name": "DotnetAPIName",
                "os": "Linux",
                "resourcegroup": "group-name",
                "runtime_version": "dotnetcore|8.0",
                "runtime_version_detected": "8.0",
                "sku": "FREE",
                "src_path": "C:\\local-path\\DotnetAPI"
            }
            ```
    -   If you need to re-deploy your app, you can use the same command as before
