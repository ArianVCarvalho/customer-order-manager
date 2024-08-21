using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Models;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Repositories;
using Frenet.ShipManagement.Services.Interface;
using Frenet.ShipManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System.Text;
using System.Text.Json;
using NLog;
using LogLevel = NLog.LogLevel;

var builder = WebApplication.CreateBuilder(args);

var config = new NLog.Config.LoggingConfiguration();

// Targets where to log to: File and Console
var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

// Rules for mapping loggers to targets            
config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

// Apply config           
NLog.LogManager.Configuration = config;

// Configura o contexto do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("FrenetShipManagementContext") ?? throw new InvalidOperationException("Connection string 'FrenetShipManagementContext' not found.")));

// Configura identidade e autenticação
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Configura autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    var configuration = builder.Configuration.GetSection("Authentication");

    var symmetricKeyAsBase64 = configuration["Secret"];
    var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
    var signingKey = new SymmetricSecurityKey(keyByteArray);

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,

        ValidateIssuer = true,
        ValidIssuer = configuration["Issuer"],

        ValidateAudience = true,
        ValidAudience = configuration["Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };

    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = tokenValidationParameters;
});

// Registra serviços e repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddHostedService<UserIdentitySeedService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication"));
builder.Services.AddHttpClient<IShippingService, ShippingService>();

// Configuração para usar camelCase e ignorar valores nulos no JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Configuração do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gerenciamento de Pedidos para Logística",
        Version = "v1",
        Description = "Desenvolver uma aplicação de gerenciamento de pedidos que permita a criação, " +
                      "atualização, visualização e exclusão de pedidos de transporte. O sistema deve incluir funcionalidades para gerenciar informações de clientes, " +
                      "status de pedidos e integração com um serviço de terceiros para cálculo de frete."
    });

    c.DescribeAllParametersInCamelCase();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o JWT Bearer token no campo abaixo usando o formato **{token}**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
var app = builder.Build();

// Configure o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Gerenciamento de Pedidos para Logística v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
