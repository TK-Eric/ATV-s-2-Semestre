using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConection")));

builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();

builder.Services.AddControllers();

var app = builder.Build();

//Mapeia as rotas definidas nos controlers com os atributos [Route}: api/[controller}
app.MapControllers();

app.Run();
