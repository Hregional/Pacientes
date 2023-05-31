using DatosPacientes.Configuration;
using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// Acceder a la configuración de la sección AdminApiConfiguration
var adminApiConfiguration = builder.Configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();

builder.Services.AddSingleton(adminApiConfiguration);


// Add services to the container.

builder.Services.AddControllers(

    ).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<RecepcionV2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnDatabase")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add security definition
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc(adminApiConfiguration.ApiVersion, new OpenApiInfo { Title = adminApiConfiguration.ApiName, Version = adminApiConfiguration.ApiVersion });

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                    TokenUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/token"),
                    Scopes = new Dictionary<string, string> {
                                { adminApiConfiguration.OidcApiName, adminApiConfiguration.ApiName }
                            }
                }
            }
        });
        options.OperationFilter<AuthorizeCheckOperationFilter>();
    });

builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = adminApiConfiguration.IdentityServerBaseUrl;
        options.RequireHttpsMetadata = true;
        options.Audience = adminApiConfiguration.OidcApiName;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
