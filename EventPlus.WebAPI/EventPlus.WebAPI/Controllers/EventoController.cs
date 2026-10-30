using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEvento _event;
        private readonly ICloudinaryService _cloudinaryServices;

        public EventController(IEvento evento, ICloudinaryService cloudinaryService)
        {
            _event = evento;
            _cloudinaryServices = cloudinaryService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Cadastrar([FromForm] EventoDTO dto)
        {
            try
            {
                string? imagemUrl = dto.ImagemUrl;

                if (dto.ArquivoImagem is not null)
                {
                    imagemUrl = await _cloudinaryServices.UploadImagem(dto.ArquivoImagem);
                }

                var evento = new Evento
                {
                    NomeEvento = dto.NomeEvento,
                    Descricao = dto.Descricao,
                    DataEvento = dto.DataEvento,
                    ImagemUrl = imagemUrl,
                    IdTipoEvento = dto.IdTipoEvento,
                    IdInstituicao = dto.IdInstituicao,
                };

                await _event.Cadastrar(evento);

                return StatusCode(201, evento);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:Guid}")]
        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromForm] EventoDTO dto)
        {
            try
            {
                string? imagemUrl = dto.ImagemUrl;

                if (dto.ArquivoImagem is not null)
                {
                    imagemUrl = await _cloudinaryServices.UploadImagem(dto.ArquivoImagem);
                }

                var evento = new Evento
                {
                    NomeEvento = dto.NomeEvento,
                    Descricao = dto.Descricao,
                    DataEvento = dto.DataEvento,
                    ImagemUrl = imagemUrl,
                    IdTipoEvento = dto.IdTipoEvento,
                    IdInstituicao = dto.IdInstituicao,
                };

                await _event.Atualizar(id, evento);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _event.Deletar(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await _event.Listar());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ListarProximos")]
        public async Task<IActionResult> ListarProximos()
        {
            try
            {
                return Ok(await _event.ListarProximos());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var evento = await _event.BuscarPorId(id);
                if (evento == null)
                {
                    return NotFound("Evento não encontrado.");
                }

                return Ok(evento);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
