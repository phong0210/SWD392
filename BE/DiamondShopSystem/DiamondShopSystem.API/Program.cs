using DiamondShopSystem.API.Shared.Responses;
using DiamondShopSystem.BLL.Utils;
using DiamondShopSystem.DAL.Data.Entities;
using DiamondShopSystem.DAL.Repositories.Impelements;
using DiamondShopSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

DotNetEnv.Env.Load(); // Load .env variables
var builder = WebApplication.CreateBuilder(args);

// Register services
ConfigureServices();
ConfigureDatabase();
ConfigureAuthentication();
ConfigureSwagger();

var app = builder.Build();

// Apply EF Core Migrations at startup if enabled
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();
    var applyMigrations = configuration.GetValue<bool>("ApplyMigrations");

    if (applyMigrations)
    {
        var dbContext = services.GetRequiredService<DiamondShopDbContext>();
        dbContext.Database.Migrate(); // This will create and migrate the DB
    }
}

// Configure middleware
ConfigureMiddleware();

app.Run();

void ConfigureMiddleware()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseHttpsRedirection();

    app.UseCors(options =>
    {
        options.SetIsOriginAllowed(origin =>
            origin.StartsWith("http://localhost:") ||
            origin.StartsWith("https://localhost:") ||
            origin.EndsWith(".vercel.app"))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "DiamondShopSystem.API",
            Version = "v1",
            Description = "A Diamond Shop System Project"
        });

        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Enter your JWT token in this format: **Bearer {your_token}**"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },
                    Scheme = "Oauth2",
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                },
                new List<string>()
            }
        });
    });
}

void ConfigureAuthentication()
{
    var configuration = builder.Configuration;
    var jwtSettings = new JWTSetting
    {
        Issuer = configuration["Jwt:Issuer"],
        Audience = configuration["Jwt:Audience"],
        Key = configuration["Jwt:Key"],
        RefreshTokenValidityInDays = int.Parse(configuration["Jwt:RefreshTokenValidityInDays"] ?? "7"),
        TokenValidityInMinutes = int.Parse(configuration["Jwt:TokenValidityInMinutes"] ?? "30")
    };

    builder.Services.AddSingleton(jwtSettings);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(token) && !token.StartsWith("Bearer "))
                    {
                        context.Request.Headers["Authorization"] = "Bearer " + token;
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var response = new ApiResponse<object>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "Unauthorized access",
                        Reason = "Authentication failed. Please provide a valid token.",
                        IsSuccess = false,
                        Data = new
                        {
                            Path = context.Request.Path,
                            Method = context.Request.Method,
                            Timestamp = DateTime.UtcNow
                        }
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            };
        });

    builder.Services.AddAuthorization();
}

void ConfigureServices()
{
    // Add controllers and configure JSON serialization (e.g. enums as strings)
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    // Swagger, HTTP context, and AutoMapper
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Unit of Work pattern
    builder.Services.AddScoped<IUnitOfWork<DiamondShopDbContext>, UnitOfWork<DiamondShopDbContext>>();

    // OTPUtil and JwtUtil - use AddScoped or AddTransient unless you intentionally want single instances
    builder.Services.AddScoped<IOTPUtil, OTPUtil>();   // Register via interface
    builder.Services.AddScoped<IJWTUtil, JwtUtil>();


    // Register your application-specific services
    RegisterApplicationServices();

    // Optional: Customize API behavior (e.g., disable automatic 400 responses)
    // builder.Services.Configure<ApiBehaviorOptions>(options =>
    // {
    //     options.SuppressModelStateInvalidFilter = true;
    // });
}


void RegisterApplicationServices()
{
    // Example:
    // builder.Services.AddScoped<IUserService, UserService>();
}

void ConfigureDatabase()
{
    var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                           $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                           $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                           $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                           $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

    builder.Services.AddDbContext<DiamondShopDbContext>(options =>
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });
    });
}
