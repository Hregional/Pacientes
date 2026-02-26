using DatosPacientes.Configuration;
using DatosPacientes.Helpers.DatosPacientes.Helpers;
using DatosPacientes.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Cargar secretos adicionales si existen
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// Configuración de Keycloak (puedes ajustarlo en appsettings o secrets.json)
var keycloakAuthority = builder.Configuration["Keycloak:Authority"] ?? "http://localhost:8080/realms/master";
var keycloakAudience = builder.Configuration["Keycloak:Audience"] ?? "account";
var requireHttps = builder.Configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata");

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

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configuración de Autenticación con Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakAuthority;
        options.Audience = keycloakAudience;
        options.RequireHttpsMetadata = requireHttps;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Datos Pacientes API v1");
        // Opcional: Configurar el Client ID por defecto para la UI de Swagger
        options.OAuthClientId(keycloakAudience);
        options.OAuthAppName("Datos Pacientes - Keycloak");
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
