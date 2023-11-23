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
-   Install the helper packages to stablish the connection

    ```SHELL
    dotnet add package Microsoft.Data.SqlClient
    dotnet add package Dapper
    dotnet add package AutoMapper
    ```

-   Configure the [data context](../DotnetAPI/Data/DataContext.cs) with Dapper or Entity Framework
-   And test it in your API

    ```CSHARP
    using Microsoft.AspNetCore.Mvc;
    using DotnetAPI.Data;
    using DotnetAPI.Models;

    namespace DotnetAPI.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _data;
        public UserController(IConfiguration config)
        {
            _data = new DataContext(config); // Inject the config to the data context to get the connection string
        }

        [HttpGet("test-connection")]
        public DateTime TestConnection()
        {
            return _data.LoadDataSingle<DateTime>("SELECT GETDATE()"); // Test the connection with a simple query that returns a single value (DateTime)
        }
    }
    ```

-   Creating models for our database entities

    -   It's a good practice to create our models as `public class partial` in case we need to extend them later

        ```CSHARP
        // .NET 8.0 Approach with first class constructor
        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;

        namespace DotnetAPI.Models;

        public partial class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UserId { get; set; }
            public string FirstName { get; set; } = ""; // Avoids null warnings
            public string LastName { get; set; } = "";
            public string Name { get { return FirstName + " " + LastName; } }
            public string Email { get; set; } = "";
            public string Gender { get; set; } = "";
            public bool Active { get; set; } = false;
        };
        ```

-   Connecting the endpoints with our DB and our Models

    ```CSHARP
    [HttpGet()] // Route path inside the parenthesis
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT TOP (10) UserId
                ,FirstName
                ,LastName
                ,Email
                ,Gender
                ,Active
            FROM TutorialAppSchema.Users";
        IEnumerable<User> users = _data.LoadData<User>(sql);
        return users;
    }
    ```

## DTOs

-   They are like models but they are used to transfer data between the client and the server
-   They are useful to hide sensitive data from the client like passwords or to avoid sending unnecessary data to the client

    ```CSHARP
    namespace DotnetAPI.Dtos;

    public partial class UserDto
    {
        // Almost the same as user but without ID
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool Active { get; set; }
    }
    ```

## Namespaces

-   They are useful to organize our code and avoid name collisions
-   They help to optimize the use of memory since they are loaded on demand

    ```CSHARP
    using DotnetAPI.Data;
    using DotnetAPI.Models;
    using DotnetAPI.Dtos;
    // The bigger the App the more specific the namespaces should be
    //
    ```

## Entity Framework

-   It's an ORM (Object Relational Mapper) that allows us to work with our database as if it were an object
-   It is useful to avoid writing SQL queries but at the same time it's not as flexible as Dapper
-   It might slow down the app since it's a heavy library and it can be a pain to configure and maintain
-   It's useful for small projects or when we don't need to write complex queries

    ```CSHARP
    using DotnetAPI.Models;
    using Microsoft.EntityFrameworkCore;

    namespace DotnetAPI.Data;

    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;
        private readonly string? _connectionString;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        // Needed to map the data models to the database
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSalary> UserSalary { get; set; }
        public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Needs Microsoft.EntityframeworkCore.SqlServer which allows to use SQL Server with EF
            if (!optionsBuilder.IsConfigured) // Needed to avoid error when running migrations
            {
                optionsBuilder.UseSqlServer(
                    _connectionString,
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure() // Needed to retry connection if it fails
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Needs Microsoft.EntityframeworkCore.Relational which allows to use relational databases with EF
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId); // Primary key is UserId
            modelBuilder.Entity<UserSalary>().HasKey(us => us.UserId);
            modelBuilder.Entity<UserJobInfo>().HasKey(uji => uji.UserId);
        }
    }
    ```

-   Using EF and AutoMapper

    ```CSHARP
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using DotnetAPI.Data;
    using DotnetAPI.Models;
    using DotnetAPI.Dtos;

    namespace DotnetAPI.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class UsersEFController(IConfiguration config) : ControllerBase
    {
        private readonly DataContextEF _data = new(config);
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserAddDto, User>();
            cfg.CreateMap<User, User>();
        }).CreateMapper();

        [HttpPost()]
        public IActionResult AddUser(UserAddDto user)
        {
            var userDb = _mapper.Map<User>(user); // Map the DTO to the model
            _data.Add(userDb);
            if (_data.SaveChanges() > 0) return Ok();
            throw new Exception("Could not add user"); ;
        }

        [HttpPut()]
        public IActionResult EditUser(User user)
        {
            string errMessage = "Could not edit user";
            User? userDb = _data.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();
            if (userDb != null)
            {
                _mapper.Map(user, userDb); // Map the new user data to the one in the database
                if (_data.SaveChanges() > 0) return Ok();
                else return BadRequest(errMessage);
            }
            throw new Exception(errMessage);
        }
    }
    ```
