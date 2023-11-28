using Microsoft.AspNetCore.Mvc.ApplicationModels;
using dotenv.net;
using DotnetAPI.Utils;
using DotnetAPI.Data;

DotEnv.Load();
var envVars = DotEnv.Read();
var PasswordKey = envVars["PASSWORD_KEY"];

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
    // Routes with hyphens
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()))
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
// Lowercase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Allows the use of the IConfiguration interface
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCorsPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCorsPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
