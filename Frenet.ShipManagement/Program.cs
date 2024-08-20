using Frenet.ShipManagement.Data;
using Frenet.ShipManagement.Repositories;
using Frenet.ShipManagement.Repositories.Interface;
using Frenet.ShipManagement.Services;
using Frenet.ShipManagement.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configura o contexto do banco de dados
builder.Services.AddDbContext<FrenetShipManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FrenetShipManagementContext")
    ?? throw new InvalidOperationException("Connection string 'FrenetShipManagementContext' not found.")));

// Configura autentica��o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Registra servi�os e reposit�rios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Registra o HttpClient para o ShippingService
builder.Services.AddHttpClient<IShippingService, ShippingService>();

builder.Services.AddScoped<TokenService>();

// Configura��o para usar camelCase e ignorar valores nulos no JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Configura��o do Swagger para documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gerenciamento de Pedidos para Log�stica",
        Version = "v1",
        Description = "Desenvolver uma aplica��o de gerenciamento de pedidos que permita a cria��o, " +
        "atualiza��o, visualiza��o e exclus�o de pedidos de transporte. O sistema deve incluir funcionalidades para gerenciar informa��es de clientes, " +
        "status de pedidos e integra��o com um servi�o de terceiros para c�lculo de frete."
    });

    // Personalize a apar�ncia dos par�metros
    c.DescribeAllParametersInCamelCase();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o JWT Bearer token no campo abaixo usando o formato **Bearer {token}**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Adiciona o requisito de seguran�a globalmente
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});


var app = builder.Build();

// Configure o pipeline de requisi��o HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ativa autentica��o e autoriza��o
app.UseAuthentication();  // Certifique-se de que este middleware seja usado
app.UseAuthorization();

app.MapControllers();

app.Run();
