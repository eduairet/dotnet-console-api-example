# Config

-   We can create a config file for our project called `appsettings.json`
-   This file contains values that we might need in several places of the app, like the connection string
    ```JSON
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;"
        }
    }
    ```
    -   We should add this to our `.csproj` file to avoid errors
        ```XML
        <None Update="appsettings.json">
            <CopyToOutputDirectory>Always</CopyToOutputDirectory>
        </None>
        ```
-   In order to read the values from this file we'll need to install the configuration package and refresh the project

    ```SHELL
    dotnet add package Microsoft.Extensions.Configuration.Json
    dotnet restore
    ```

-   Now we can set our configuration
    ```CSHARP
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build(); // Loads appsettings.json and returns the configuration with .Build()
    ```
-   And use it

    ```CSHARP
    public class DataContextDapper
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql);
        }
    }

    DataContextDapper dataContextDapper = new(config); // config from above
    ```
