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
-   SQL Database
    -   In Azure, create a SQL Database (paid service only)
    -   Select the resource group and subscription
    -   Name it and select the server
        -   If there is no server create it
            -   Add the server name and the closest location to you
            -   Select the authentication method, in this case SQL Authentication
                -   Add the username and password
    -   Select the pricing tier configuration
    -   Select the backup storage redundancy
    -   Next we're going to setup the networking
        -   The connectivity method is public endpoint since we're testing
        -   The same with the firewall rules
            -   We're going to allow access to Azure services and resources even though it's not recommended for a real app
            -   The current client IP address can added both for testing and production
        -   We can leave the connection policy and TLS version as default
    -   We don't need to change anything in the security step for this test but i n a real app we should set the security recommendations
    -   We're leaving additional settings and tags as default
    -   And finally we're going to review and create the database
    -   Once the database is created, we're going to copy the server name and go to azure studio
        -   We're going to connect to the server using the server name, the username and password
    -   Once connected with the server we can populate the database with the tables and data
        -   We can use the SQL script to generate the tables and data
