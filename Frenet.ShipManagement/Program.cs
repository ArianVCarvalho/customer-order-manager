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

// Configura autenticação JWT
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

// Registra serviços e repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Registra o HttpClient para o ShippingService
builder.Services.AddHttpClient<IShippingService, ShippingService>();

builder.Services.AddScoped<TokenService>();

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

    // Personalize a aparência dos parâmetros
    c.DescribeAllParametersInCamelCase();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o JWT Bearer token no campo abaixo usando o formato **Bearer {token}**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Adiciona o requisito de segurança globalmente
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

// Configure o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ativa autenticação e autorização
app.UseAuthentication();  // Certifique-se de que este middleware seja usado
app.UseAuthorization();

app.MapControllers();

app.Run();
