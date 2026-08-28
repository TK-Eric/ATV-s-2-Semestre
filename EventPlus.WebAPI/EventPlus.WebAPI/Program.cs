using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Services;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

// Ponto de partida da aplicação: aqui é onde tudo é configurado
// antes da API começar a rodar de verdade
var builder = WebApplication.CreateBuilder(args);

//Adicionando Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira um token válido para ter acesso aos endpoints da API"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// Registra o EventContext (a "porta" pro banco de dados) e diz pra ele
// usar SQL Server, pegando a string de conexão do appsettings.json
builder.Services.AddDbContext<EventContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DeafaultConnection")));

// Habilita o uso de Controllers (as classes tipo EventController)
// e configura o JSON: "IgnoreCycles" evita erro quando duas entidades
// se referenciam uma à outra (ex: Evento tem lista de Comentario, e
// Comentario referencia de volta o Evento — sem isso, o JSON entraria
// em loop infinito tentando serializar)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Injeção de dependência: toda vez que alguém pedir uma interface
// (tipo IEvent), o sistema entrega automaticamente a classe que
// implementa ela (tipo EventRepository).
// "AddScoped" = uma instância nova é criada a cada requisição
builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IComentario, ComentarioRepository>();
builder.Services.AddScoped<IEvent, EventoRepository>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// Configura a autenticação da API usando JWT (token de acesso)
builder.Services.AddAuthentication(options =>
{
    // Define o JWT Bearer como esquema padrão pra autenticar
    // e pra desafiar (pedir login) o usuário
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        // Regras usadas pra validar se um token JWT é válido
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Confere se o token foi emitido por quem devia (o "Issuer")
            ValidateIssuer = true,
            ValidIssuer = "EventPlus.WebAPI",

            // Confere se o token foi feito pra essa API (o "Audience")
            ValidateAudience = true,
            ValidAudience = "EventPlus.WebAPI",

            // Confere se o token ainda não expirou
            ValidateLifetime = true,

            // Dá uma "folga" de 10 minutos na validação do tempo de
            // expiração (compensa pequenas diferenças de horário entre servidores)
            ClockSkew = TimeSpan.FromMinutes(10),

            // Confere se o token foi realmente assinado com a chave certa
            // (evita que alguém forje um token falso)
            ValidateIssuerSigningKey = true,

            // Chave usada pra validar a assinatura do token.
            // Agora lê o valor de verdade do appsettings.json (seção
            // "Jwt" -> "Key"), em vez de usar o texto "Jwt:Key" literal
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

// Configuração do Cloudinary: pega os dados da seção "Cloudinary"
// do appsettings.json (chave de API, nome da conta, etc) e disponibiliza
// como CloudinarySettings pra ser injetado onde precisar
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

// A partir daqui, monta de fato a aplicação com tudo que foi configurado
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Força todo mundo a usar HTTPS (redireciona automaticamente de HTTP pra HTTPS)
app.UseHttpsRedirection();

// Ativa a verificação de QUEM é o usuário (autenticação = "quem você é")
app.UseAuthentication();

// Ativa a verificação do QUE o usuário pode fazer (autorização = "o que você pode acessar")
app.UseAuthorization();

// Liga as rotas dos Controllers (ex: api/Event) pra API responder às requisições
app.MapControllers();

// Inicia a aplicação e deixa ela "escutando" requisições
app.Run();