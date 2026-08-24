using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    // O endereço desse controller vai ser "api/Event"
    [Route("api/[controller]")]
    // Isso aqui avisa que essa classe é uma API (ativa umas verificações
    // automáticas, tipo checar se os dados enviados estão certos)
    [ApiController]
    public class EventController : ControllerBase
    {
        // Essas duas variáveis guardam "ferramentas" que a classe vai usar:
        private readonly IEvent _event; // pra salvar/mexer com eventos
        private readonly ICloudinaryService _cloudinaryServices; // pra subir imagem pra internet (Cloudinary)

        // Aqui o C# entrega essas ferramentas prontas quando o controller é criado
        public EventController(IEvent evento, ICloudinaryService cloudinaryService)
        {
            _event = evento;
            _cloudinaryServices = cloudinaryService;
        }

        // Esse método responde quando alguém manda um POST pra "api/Event"
        [HttpPost]
        // Avisa que vai receber um formulário (texto + arquivo juntos)
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Cadastrar([FromForm] EventDTO dto)
        {
            // O "dto" já chega preenchido com os dados que o usuário mandou no formulário

            try
            {
                // Começa vazio, só vai ter valor se o usuário mandar uma imagem
                string? imagemUrl = null;

                // Verifica se o usuário enviou algum arquivo de imagem
                if (dto.ArquivoImagem is not null)
                {
                    // Se enviou, sobe a imagem pro Cloudinary
                    // e guarda o link (URL) que ele devolve
                    imagemUrl = await _cloudinaryServices.UploadImagem(dto.ArquivoImagem);
                }

                // Cria um novo evento, pegando os dados que vieram do formulário
                var evento = new Evento
                {
                    NomeEvento = dto.NomeEvento,
                    Descricao = dto.Descricao,
                    DataEvento = dto.DataEvento,
                    ImagemUrl = imagemUrl, // link da imagem (ou nulo, se não mandou imagem)
                    IdTipoEvento = dto.IdTipoEvento,
                    IdInstituicao = dto.IdInstituicao,
                };

                // Manda esse evento pra ser salvo no banco de dados
                await _event.Cadastrar(evento);

                // Deu tudo certo! Retorna status 201 (criado com sucesso)
                // e devolve o evento criado (já com o Id gerado pelo banco)
                return StatusCode(201, evento);
            }
            catch (Exception e)
            {
                // Se der algum erro no meio do caminho (upload falhou, banco deu erro, etc)
                // retorna status 400 (deu ruim) junto com a mensagem do erro
                return BadRequest(e.Message);
            }
        }
    }
}