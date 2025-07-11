using System.Collections;
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Repositories;
using DiamondShopSystem.BLL.Mapping;
using MediatR;
using AutoMapper;
using DiamondShopSystem.BLL.Handlers;
using DiamondShopSystem.API.Policies;
using DiamondShopSystem.BLL.Handlers.User;
using DiamondShopSystem.BLL.Services;
using DiamondShopSystem.BLL.Handlers.User.Validators;
using DiamondShopSystem.BLL.Services.Auth;
using DiamondShopSystem.BLL.Services.User;
using DiamondShopSystem.BLL.Services.Order;


DotNetEnv.Env.Load(Path.Combine("..", "..", ".env")); // Load from parent of API folder

var builder = WebApplication.CreateBuilder(args);

// Modular service registration
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
        var dbContext = services.GetRequiredService<AppDbContext>();
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

    // Add your custom error handling middleware if needed
    // app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.UseCors("AllowAll"); // Or your named policy

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

void ConfigureServices()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAutoMapper(typeof(EntityToDtoProfile).Assembly);
    builder.Services.AddMediatR(typeof(DiamondShopSystem.BLL.Handlers.User.Validators.UserCreateValidator).Assembly);

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    // CORS Policies
    builder.Services.AddCorsPolicies();

    builder.Services.AddControllers();

    builder.Services.AddDiamondShopValidators();
    
    // Auth Services
    builder.Services.AddScoped<JwtUtil>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    
    // User Services
    builder.Services.AddScoped<IUserService, UserService>();
    
    // Order Services
    builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
}

void ConfigureAuthentication()
{
    var config = builder.Configuration;
    var jwtSecret = Environment.GetEnvironmentVariable("JWT_KEY");
    var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
    var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
    var jwtTokenValidityInMinutes = Environment.GetEnvironmentVariable("JWT_TOKEN_VALIDITY_IN_MINUTES");
    var jwtRefreshTokenValidityDays = Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_VALIDITY_IN_DAYS");

    Console.WriteLine("======Configuring authentication======");
    Console.WriteLine("JWT_KEY: " + jwtSecret);
    Console.WriteLine("JWT_ISSUER: " + jwtIssuer);
    Console.WriteLine("JWT_AUDIENCE: " + jwtAudience);
    Console.WriteLine("JWT_TOKEN_VALIDITY_IN_MINUTES: " + jwtTokenValidityInMinutes);
    Console.WriteLine("JWT_REFRESH_TOKEN_VALIDITY_IN_DAYS: " + jwtRefreshTokenValidityDays);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret ?? throw new InvalidOperationException("JWT_KEY environment variable is not set"))),
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
                    await context.Response.WriteAsync("{\"error\":\"Unauthorized access\"}");
                }
            };
        });

    builder.Services.AddRolePolicies();
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

        // JWT security for Swagger UI
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
    Console.WriteLine("======Configuring database======");
    Console.WriteLine("DB_LOCAL_HOST: " + Environment.GetEnvironmentVariable("DB_LOCAL_HOST"));
    Console.WriteLine("DB_PORT_LOCAL: " + Environment.GetEnvironmentVariable("DB_PORT_LOCAL"));
    Console.WriteLine("DB_NAME: " + Environment.GetEnvironmentVariable("DB_NAME"));
    Console.WriteLine("DB_USER: " + Environment.GetEnvironmentVariable("DB_USER"));
    Console.WriteLine("DB_PASSWORD: " + Environment.GetEnvironmentVariable("DB_PASSWORD"));

    // Read from environment variables or .env
    var host = Environment.GetEnvironmentVariable("DB_LOCAL_HOST");
    var port = Environment.GetEnvironmentVariable("DB_PORT_LOCAL");
    var database = Environment.GetEnvironmentVariable("DB_NAME");
    var username = Environment.GetEnvironmentVariable("DB_USER");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

    if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) ||
        string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        throw new InvalidOperationException("One or more required database environment variables are missing!");
    }

    var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};Client Encoding=UTF8;";

    builder.Services.AddDbContext<AppDbContext>(options =>
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