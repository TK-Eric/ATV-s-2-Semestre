using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
using Projeto_Bolos_do_Jacquin.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext<BolosContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DeafaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
//builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
//builder.Services.AddScoped<IUsuario, UsuarioRepository>();
//builder.Services.AddScoped<IComentario, ComentarioRepository>();
//builder.Services.AddScoped<IEvento, EventoRepository>();
//builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

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
            ValidIssuer = "EventPlus.WebAPI",

            ValidateAudience = true,
            ValidAudience = "EventPlus.WebAPI",

            ValidateLifetime = true,

            ClockSkew = TimeSpan.FromMinutes(10),

            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.Configure<SightengineSettings>(builder.Configuration.GetSection("Sightengine"));

builder.Services.AddHttpClient<IModerationService, SightengineModerationService>(client =>
{
    client.BaseAddress = new Uri("https://api.sightengine.com/1.0/");
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();