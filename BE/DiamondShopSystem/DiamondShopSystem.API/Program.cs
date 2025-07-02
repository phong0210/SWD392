using DiamondShopSystem.API.DTOs;
using DiamondShopSystem.API.Extensions;
using DiamondShopSystem.API.Middlewares;
using DiamondShopSystem.BLL.Application.Services.Authentication;
using DiamondShopSystem.BLL.Utils;
using DiamondShopSystem.DAL.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections;
using System.Text;

DotNetEnv.Env.Load(Path.Combine("..", "..", ".env")); 

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
        dbContext.Database.Migrate();
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

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.UseSecurityMiddleware(builder.Configuration);
    app.UseCors("SecureCorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

void ConfigureServices()
{

    builder.Services.AddExceptionHandler(options =>
    {
        options.ExceptionHandler = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An unexpected error occurred.",
                Reason = "Something went wrong on the server.",
                IsSuccess = false,
                Data = new
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Timestamp = DateTime.UtcNow
                }
            };

            await context.Response.WriteAsJsonAsync(response);
        };
    });

    // FluentValidation setup (Migrated to newer version)
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();
    builder.Services.AddValidatorsFromAssemblyContaining<DiamondShopSystem.API.Validators.RegisterUserRequestValidator>();


    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DiamondShopSystem.BLL.Application.Interfaces.IUnitOfWork).Assembly));

    builder.Services.AddScoped<DiamondShopSystem.BLL.Application.Interfaces.IUnitOfWork, DiamondShopSystem.DAL.Repositories.Implementations.UnitOfWork>();
    builder.Services.AddScoped<IOTPUtil, OTPUtil>();
    builder.Services.AddScoped<IJWTUtil, JwtUtil>();
    builder.Services.AddScoped<DiamondShopSystem.BLL.Application.Services.Authentication.IAuthenticationService, DiamondShopSystem.BLL.Application.Services.Authentication.AuthenticationService>();
    builder.Services.AddScoped<DiamondShopSystem.BLL.Application.Services.VNPay.IVNPayService, DiamondShopSystem.BLL.Application.Services.VNPay.VNPayService>();
    builder.Services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
    builder.Services.AddScoped<IAuthenticationLogger, AuthenticationLogger>();
    builder.Services.AddHttpClient<GoogleOAuthService>();
    
    // Add security services
    builder.Services.AddSecurityServices(builder.Configuration);
}

void ConfigureAuthentication()
{
    var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
    var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
    var jwtRefreshTokenValidityInDays = Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_VALIDITY_IN_DAYS") ?? "7";
    var jwtTokenValidityInMinutes = Environment.GetEnvironmentVariable("JWT_TOKEN_VALIDITY_IN_MINUTES") ?? "30";
    var config = builder.Configuration;
    var jwtSettings = new JWTSetting

    {
        Issuer = jwtIssuer,
        Audience = jwtAudience,
        Key = jwtKey ?? throw new InvalidOperationException("JWT_KEY environment variable is not set!"),
        RefreshTokenValidityInDays = jwtRefreshTokenValidityInDays != null ? int.Parse(jwtRefreshTokenValidityInDays) : 7,
        TokenValidityInMinutes = jwtTokenValidityInMinutes != null ? int.Parse(jwtTokenValidityInMinutes) : 30
    };

    Console.WriteLine("=== JWT Settings ===");
    Console.WriteLine($"Issuer: {jwtSettings.Issuer}");
    Console.WriteLine($"Audience: {jwtSettings.Audience}");
    Console.WriteLine($"Key: {(string.IsNullOrEmpty(jwtSettings.Key) ? "NULL/EMPTY" : "***SET***")}");
    Console.WriteLine($"Refresh Token Validity (Days): {jwtSettings.RefreshTokenValidityInDays}");
    Console.WriteLine($"Token Validity (Minutes): {jwtSettings.TokenValidityInMinutes}");
    Console.WriteLine("Key Length: " + (string.IsNullOrEmpty(jwtSettings.Key) ? 0 : Encoding.UTF8.GetByteCount(jwtSettings.Key)));
    Console.WriteLine("======================");

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

void ConfigureSwagger()
{
    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "DiamondShopSystem.API",
            Version = "v1",
            Description = "A Diamond Shop System Project"
        });

        // Temporarily commented out JWT security configuration
       
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
                    Scheme = "oauth2",
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });
}

void ConfigureDatabase()
{
    // Debug: Print ALL environment variables
    Console.WriteLine("=== ALL ENVIRONMENT VARIABLES ===");
    foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
    {
        if (env.Key.ToString().Contains("DB") || env.Key.ToString().Contains("JWT"))
        {
            var value = env.Value?.ToString();
            if (env.Key.ToString().Contains("PASSWORD") || env.Key.ToString().Contains("KEY"))
            {
                value = string.IsNullOrEmpty(value) ? "NULL/EMPTY" : "***SET***";
            }
            Console.WriteLine($"{env.Key}: {value}");
        }
    }
    Console.WriteLine("=====================================");

    var host = Environment.GetEnvironmentVariable("DB_LOCAL_HOST");
    var port = Environment.GetEnvironmentVariable("DB_PORT_LOCAL");
    var database = Environment.GetEnvironmentVariable("DB_NAME");
    var username = Environment.GetEnvironmentVariable("DB_USER");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

    // Add logging to see what values are being read
    Console.WriteLine($"DB_LOCAL_HOST: {host}");
    Console.WriteLine($"DB_PORT_LOCAL: {port}");
    Console.WriteLine($"DB_NAME: {database}");
    Console.WriteLine($"DB_USER: {username}");
    Console.WriteLine($"DB_PASSWORD: {password}");

    if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) ||
        string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        throw new InvalidOperationException("One or more required database environment variables are missing!");
    }

        var connectionString = $"Host={host};" +
                       $"Port={port};" +
                       $"Database={database};" +
                       $"Username={username};" +
                       $"Password={password};" +
                       "Client Encoding=UTF8;";

    Console.WriteLine($"Connection String: {connectionString}");

    builder.Services.AddDbContext<DiamondShopDbContext>(options =>
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("DiamondShopSystem.DAL");
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
    });
}

