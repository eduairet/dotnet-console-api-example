# API Basics

-   `Startup.cs` - Legacy file not present on .NET 6^
-   Controllers - Logic that process the data of our API
-   URL Parameters - The endpoints to connect with our API
-   Database connection - We'll get the data from our DB with tools like Dapper or the Entity Framework

## Starting an API

```SHELL
dotnet new webapi --name DotnetAPI
```

-   Common structure

    ```
    │   appsettings.Development.json
    │   appsettings.json
    │   DotnetAPI.csproj
    │   Program.cs
    │   WeatherForecast.cs
    │
    ├───Controllers
    │       WeatherForecastController.cs
    │
    ├───obj
    │       DotnetAPI.csproj.nuget.dgspec.json
    │       DotnetAPI.csproj.nuget.g.props
    │       DotnetAPI.csproj.nuget.g.targets
    │       project.assets.json
    │       project.nuget.cache
    │
    └───Properties
            launchSettings.json
    ```

-   `Program.cs`

    ```CSHARP
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers(); // Adds our controllers to the builder
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer(); // Add the endpoints to the app
    builder.Services.AddSwaggerGen(); // Configures Swagger

    var app = builder.Build(); // Creates our application

    // Configure the HTTP request pipeline.
    // It's set depending on the environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(); // Practical UI to test our API
    }

    app.UseHttpsRedirection(); // Forces https

    app.UseAuthorization(); //

    app.MapControllers(); // Maps our App controllers

    app.Run(); // Starts the server
    ```

-   `Controllers`

    -   They process the data and have all the functionality needed to make the API work

    -   Routes on controllers are classes that are called when the endpoint is reached by a client, so they don't need to have an instance

        ```CSHARP
        using Microsoft.AspNetCore.Mvc;

        namespace DotnetAPI.Controllers;

        [ApiController]
        [Route("[controller]")]
        public class WeatherForecastController : ControllerBase
        {
            private static readonly string[] Summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            private readonly ILogger<WeatherForecastController> _logger;

            public WeatherForecastController(ILogger<WeatherForecastController> logger)
            {
                _logger = logger;
            }

            [HttpGet(Name = "GetWeatherForecast")]
            public IEnumerable<WeatherForecast> Get()
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
        }
        ```

-   `Models`

    -   Are elements that define our data

        ```CSHARP
        namespace DotnetAPI;

        public class WeatherForecast
        {
            public DateOnly Date { get; set; }
            public int TemperatureC { get; set; }
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
            public string? Summary { get; set; }
        }
        ```
