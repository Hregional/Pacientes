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

// Cargar secretos adicionales si existen.
// appsettings.json ya lo carga CreateBuilder; las variables de entorno se vuelven a agregar
// al final para que el .env de Docker siempre tenga prioridad sobre los archivos JSON.
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configuración de Keycloak: el Authority se arma como {KEYCLOAK_URL}/realms/{KEYCLOAK_REALM}.
// KEYCLOAK_AUTHORITY (URL completa) se mantiene por compatibilidad y tiene prioridad si se define.
var keycloakUrl = (builder.Configuration["KEYCLOAK_URL"]
    ?? builder.Configuration["Keycloak:Url"]
    ?? "https://sso.hro.gob.gt").TrimEnd('/');
var keycloakRealm = builder.Configuration["KEYCLOAK_REALM"]
    ?? builder.Configuration["Keycloak:Realm"]
    ?? "interno";

var keycloakAuthority = (builder.Configuration["KEYCLOAK_AUTHORITY"]
    ?? builder.Configuration["Keycloak:Authority"]
    ?? $"{keycloakUrl}/realms/{keycloakRealm}").TrimEnd('/');

// Issuer esperado en el claim "iss" del token. Por defecto es igual al Authority; solo se
// define KEYCLOAK_PUBLIC_URL cuando la API llega a Keycloak por una URL interna distinta a la pública.
var keycloakPublicUrl = builder.Configuration["KEYCLOAK_PUBLIC_URL"]
    ?? builder.Configuration["Keycloak:PublicUrl"];
var keycloakValidIssuer = string.IsNullOrWhiteSpace(keycloakPublicUrl)
    ? keycloakAuthority
    : $"{keycloakPublicUrl.TrimEnd('/')}/realms/{keycloakRealm}";

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
                // El navegador usa la URL pública de Keycloak (la misma del issuer)
                AuthorizationUrl = new Uri($"{keycloakValidIssuer}/protocol/openid-connect/auth"),
                TokenUrl = new Uri($"{keycloakValidIssuer}/protocol/openid-connect/token"),
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakAuthority;
        options.RequireHttpsMetadata = requireHttps;
        options.MetadataAddress = $"{keycloakAuthority}/.well-known/openid-configuration";

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = keycloakValidIssuer,
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
    app.UseSwaggerUI(options =>
    {
        // Pre-rellena el client_id y client_secret en la UI de Swagger
        options.OAuthClientId(keycloakClientId);
        if (!string.IsNullOrEmpty(keycloakClientSecret))
        {
            options.OAuthClientSecret(keycloakClientSecret);
        }
        options.OAuthAppName("Swagger UI - Datos Pacientes");
         options.OAuthUsePkce();
    });
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
