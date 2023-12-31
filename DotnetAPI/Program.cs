using Microsoft.AspNetCore.Mvc.ApplicationModels;
using DotnetAPI.Utils;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:TokenKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
