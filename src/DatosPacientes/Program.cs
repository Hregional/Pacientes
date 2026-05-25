using DatosPacientes.Configuration;
using DatosPacientes.Helpers;
using DatosPacientes.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Mapster;
using MapsterMapper;

// Activar logs detallados de identidad (PII) para ver el error real (útil para depurar problemas de tokens)
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

// Cargar secretos adicionales si existen
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuración de Keycloak
var keycloakAuthority = builder.Configuration["KEYCLOAK_AUTHORITY"]
    ?? builder.Configuration["Keycloak:Authority"]
    ?? "http://192.168.1.18/realms/myrealm";


var keycloakAudience = builder.Configuration["KEYCLOAK_AUDIENCE"] ?? builder.Configuration["Keycloak:Audience"] ?? "account";
var keycloakClientId = builder.Configuration["KEYCLOAK_CLIENTID"] ?? builder.Configuration["Keycloak:ClientId"] ?? "api-pacientes";
var keycloakClientSecret = builder.Configuration["KEYCLOAK_CLIENTSECRET"] ?? builder.Configuration["Keycloak:ClientSecret"];
var requireHttps = builder.Configuration.GetValue<bool>("KEYCLOAK_REQUIREHTTPSMETADATA") || builder.Configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata");

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.Converters.Add(new DateTimeConverter("dd-MM-yyyy"));
    });

builder.Services.AddDbContext<RecepcionV2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnDatabase")));

builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger con Seguridad OAuth2/OpenID Connect
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Datos Pacientes API", Version = "v1" });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/auth"),
                TokenUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect" },
                    { "profile", "User Profile" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { "openid", "profile" }
        }
    });
});

// Configuración de Mapster
builder.Services.AddMapster();

// Configuración de Keycloak
//var keycloakAuthority = builder.Configuration["Keycloak:Authority"]
    //?? "http://192.168.1.18:8080/realms/myrealm";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakAuthority;
        options.RequireHttpsMetadata = false;
        options.MetadataAddress = $"{keycloakAuthority.TrimEnd('/')}/.well-known/openid-configuration";

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = keycloakAuthority,        // usa la variable, no hardcodeado
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "preferred_username"
        };

        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();
                logger.LogError("[Auth FAILED] Tipo: {tipo} | Mensaje: {msg}",
                    context.Exception.GetType().Name,
                    context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();
                logger.LogInformation("[Auth SUCCESS] Usuario: {user}",
                    context.Principal?.Identity?.Name);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comentado para usar solo HTTP en el contenedor y evitar la advertencia de redirección HTTPS
// app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
