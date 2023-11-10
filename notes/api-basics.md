# API Basics

-   `Startup.cs` - Legacy file not present on .NET 6^

    -   I breaks the startup logic in two files `Program` and `Startup`

        ```CSHARP
        using Microsoft.AspNetCore.Builder;
        using Microsoft.Extensions.DependencyInjection;
        using Microsoft.Extensions.Hosting;

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                // Configure services here
            }

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                // Configure middleware and routes here
            }
        }
        ```

        ```CSHARP
        using Microsoft.AspNetCore.Hosting;
        using Microsoft.Extensions.Hosting;

        public class Program
        {
            public static void Main(string[] args)
            {
                CreateHostBuilder(args).Build().Run();
            }

            public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
        }
        ```

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

## Running the App

```SHELL
dotnet run
# With hot reload
dotnet watch run
```

-   The previous command will start our server

    ```SHELL
    Building...
    info: Microsoft.Hosting.Lifetime[14]
        Now listening on: http://localhost:5202
    info: Microsoft.Hosting.Lifetime[0]
        Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
        Hosting environment: Development
    info: Microsoft.Hosting.Lifetime[0]
        Content root path: C:\Users\User\DotnetAPI
    ```

-   Now we can use the API
    -   The swagger UI is in the same URL than the server, we just need to add the swagger UI home route `http://localhost:5202/swagger/index.html`

## Settings

-   `DotnetAPI\Properties\launchSettings.json` - Is useful to set our dev port
    ```JSON
    {
      "$schema": "http://json.schemastore.org/launchsettings.json",
      "iisSettings": {
        "windowsAuthentication": false,
        "anonymousAuthentication": true,
        "iisExpress": {
          "applicationUrl": "http://localhost:59036",
          "sslPort": 44360
        }
      },
      "profiles": {
        "http": {
          "commandName": "Project",
          "dotnetRunMessages": true,
          "launchBrowser": true,
          "launchUrl": "swagger",
          "applicationUrl": "http://localhost:5000",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        },
        "https": {
          "commandName": "Project",
          "dotnetRunMessages": true,
          "launchBrowser": true,
          "launchUrl": "swagger",
          "applicationUrl": "https://localhost:5001;http://localhost:5000",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        },
        "IIS Express": {
          "commandName": "IISExpress",
          "launchBrowser": true,
          "launchUrl": "swagger",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          }
        }
      }
    }
    ```
-   ``CORS` - This is to avoid network errors (different for production)

    ```CSHARP
    // ...

    // Cross Origin Policy
    builder.Services.AddCors(options =>
    {
        // Dev
        options.AddPolicy("DevCorsPolicy", policy =>
        {
            // Specify the allowed origins (usually frontend)
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod() // Allow any HTTP method: POST, PUT...
                .AllowAnyHeader() // Allow any HTTP header
                .AllowCredentials(); // Adjust as per your requirements
        });
        // Production
        options.AddPolicy("ProdCorsPolicy", policy =>
        {
            // Specify the allowed prod origins (deployed app domains)
            policy.WithOrigins("https://www.example.com")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseCors("DevCorsPolicy");
        // ...
    }
    else
    {
        app.UseCors("ProdCorsPolicy");
    }
    // ...
    ```

## Database connection

-   Set `appsettings.json` first
    ```JSON
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;"
        },
        "Logging": {
            "LogLevel": {
                "Default": "Information",
                "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "Kestrel": {
            "Endpoints": {
                "Https": {
                    "Url": "https://localhost:5001"
                }
            }
        }
    }
    ```
-   Once the connection string is set it can be accessible all over the app
    ```CSHARP
    public UserController(IConfiguration config)
    {
        Console.WriteLine(config.GetConnectionString("DefaultConnection"));
    }
    ```
