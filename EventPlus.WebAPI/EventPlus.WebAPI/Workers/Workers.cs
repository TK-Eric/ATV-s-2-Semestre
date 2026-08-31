//using EventPlus.WebAPI.BdContextEvent;
//using EventPlus.WebAPI.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace EventPlus.WebAPI.Workers
//{
//    public class Worker : BackgroundService
//    {
//        private readonly IServiceScopeFactory _scopeFactory;
//        private readonly ILogger<Worker> _logger;

//        public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger)
//        {
//            _scopeFactory = scopeFactory;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("Worker Iniciado");

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                using (var scope = _scopeFactory.CreateScope())
//                {
//                    var _context = scope.ServiceProvider.GetRequiredService<EventContext>();
//                    var _enviarEmail = scope.ServiceProvider.GetRequiredService<IEnviarEmailService>();

//                    var usuariosEnviarEmail = await _context.Presenca.Include(u => u.IdUsuarioNavigation).Include(e => e.IdEventoNavigation).ThenInclude(e => e.IdStatusEventoNavigation).Where(p => p.IdEventoNavigation.DataEvento <= DateTime.UtcNow.AddDays(1) && p.IdEventoNavigation.DataEvento >= DateTime.UtcNow && p.IdEventoNavigation.IdStatusEventoNavigation.Status == false).ToListAsync(stoppingToken);

//                    _logger.LogInformation($"Contador: {usuariosEnviarEmail.Count()}");

//                    foreach (var u in usuariosEnviarEmail)
//                    {
//                        var assunto = $"Lembrete: {u.IdEventoNavigation.NomeEvento} é amanhã";
//                        var corpo =
//                            $@"<h2>Olá, {u.IdUsuarioNavigation.Nome}!</h2>
//<p>Seu Evento: <strong>{u.IdEventoNavigation.NomeEvento}</strong> está agendado para {u.IdEventoNavigation.DataEvento:dd-MM-yyyy HH:mm}.</p>
//<p>Não esqueça de comparecer!</p>
//<p>Este é um envio de email automático, não responda!</p>";

//                        await _enviarEmail.EnviarEmailAsync(assunto: assunto, corpo: corpo, destinatario: u.IdUsuarioNavigation.Email, ct: stoppingToken);

//                        u.IdEventoNavigation.IdStatusEvento = Guid.Parse("1D7E6888-E59C-44AB-ADD6-6D20AD7F862E");
//                    }

//                    await _context.SaveChangesAsync(stoppingToken);

//                }
//                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
//            }
//        }
//    }
//}